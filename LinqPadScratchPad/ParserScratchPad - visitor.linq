<Query Kind="Statements">
  <Reference Relative="..\Lumbre.Interfaces\bin\Debug\net8.0\Lumbre.Interfaces.dll">C:\Users\Tomas\source\repos\Lumbre\Lumbre.Interfaces\bin\Debug\net8.0\Lumbre.Interfaces.dll</Reference>
  <Namespace>Hl7.Fhir.ElementModel</Namespace>
  <Namespace>Hl7.Fhir.FhirPath</Namespace>
  <Namespace>Hl7.Fhir.Model</Namespace>
  <Namespace>Hl7.Fhir.Rest</Namespace>
  <Namespace>Hl7.Fhir.Serialization</Namespace>
  <Namespace>Hl7.Fhir.Specification</Namespace>
  <Namespace>Hl7.Fhir.Specification.Source</Namespace>
  <Namespace>Lumbre.Interfaces.Common</Namespace>
  <Namespace>Lumbre.Interfaces.Contracts</Namespace>
  <Namespace>Lumbre.Interfaces.Query</Namespace>
  <Namespace>Lumbre.Interfaces.Query.Definitions</Namespace>
  <Namespace>Lumbre.Interfaces.Repository</Namespace>
  <Namespace>System.Diagnostics.CodeAnalysis</Namespace>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

using Expression = System.Linq.Expressions.Expression;

Expression<Func<PatientQuery, bool>> exp = x => x.Organization == "name"  && (x.LastUpdated < new DateTime(2024, 05, 03) || x.Id == "1");
var queries = new List<ISearchExpression>();
exp.Body.Dump();

queries.Dump();

var visitor = new FHIRExpressionVisitor(); 
visitor.Visit(exp);

visitor.Queries.Dump();



public enum LogicalGroupOperand {
	And, Or
}

public enum Operator {
	Equals, GT, LT, GTE, LTE, Contains
}

public interface ISearchExpression;
public class SimpleExpression() : ISearchExpression {
	public ExpressionType Op { get; set; }
	public string Path { get; set; }
	public Object ExpectedValue { get; set; } 
}


public class ExpressionGroup() : ISearchExpression {
	public LogicalGroupOperand Op { get; set; }
	public IList<ISearchExpression> Expressions { get; set;} = new List<ISearchExpression>();
};

public class FHIRExpressionVisitor : ExpressionVisitor
{
	public List<ISearchExpression> Queries { get; set; } = new();

	private SimpleExpression _currentSimpleExpression = null;
	public Stack<ExpressionGroup> Stack { get; set; } = new();


	protected override Expression VisitBinary(BinaryExpression node)
	{

		if (node.NodeType.In(ExpressionType.AndAlso, ExpressionType.OrElse))
		{
			var newGroup = new ExpressionGroup();
			Stack.Push(newGroup);
			newGroup.Op = node.NodeType.ToLogical();
			Visit(node.Left);
			Visit(node.Right);
			Queries.Add(newGroup);
			return node;
		}

		_currentSimpleExpression = new SimpleExpression();
		_currentSimpleExpression.Op = node.NodeType;

		Visit(node.Left);
		Visit(node.Right);
		if (Stack.Count == 0) {
			Queries.Add(_currentSimpleExpression);
			return node;
		}
		var current = Stack.Pop();
		current.Expressions.Add(_currentSimpleExpression);
		Stack.Push(current);
		return node;
	}

	protected override Expression VisitConstant(ConstantExpression node)
	{
		if (node.Value is Expression value)
		{
			Visit(value);
		}
		else
		{
			_currentSimpleExpression.ExpectedValue = node.Value;
		}
		return node;
	}


	protected override Expression VisitMember(MemberExpression node)
	{
		if (node.Expression != null)
		{
			Visit(node.Expression);
		}

		var attr = node.Member.GetCustomAttribute<Lumbre.Interfaces.Query.Metadata.JsonPathAttribute>();
		if (attr == null)
			return node;
		var path = attr.Path;

		_currentSimpleExpression.Path = path;

		return node;
	}

	protected override Expression VisitMethodCall(MethodCallExpression node)
	{
		return node;
	}


	protected override Expression VisitNew(NewExpression node)
	{
		// Compile the NewExpression into a lambda, then compile that into a delegate
		var converted = Expression.Convert(node, typeof(object));
		var lambda = Expression.Lambda<Func<object>>(converted);
		var compiled = lambda.Compile();

		// Execute the delegate to create the object
		var newObject = compiled();
		_currentSimpleExpression.ExpectedValue = newObject;
		return node;
	}
}

public static class ExtUtils {

	public static bool In<T>(this T value, params System.Enum[] values)  where T: System.Enum 
		=> values.Any(v => v.Equals(value));
		
	public static LogicalGroupOperand ToLogical<T>(this T value) where T: System.Enum => value switch {
		ExpressionType.AndAlso => LogicalGroupOperand.And,
		ExpressionType.OrElse => LogicalGroupOperand.Or,
	};
}