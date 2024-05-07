using Lumbre.Interfaces.Query.Definitions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lumbre.Middleware.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Lumbre.Interfaces.Query.Descriptors;

namespace Testing.Misc
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void CanParseSimple() 
        {
            Expression<Func<PatientQuery, bool>> exp = x => x.Organization == "ID001"; //&& x.LastUpdated < new DateTime(2024, 05, 03) && x.Id == "1";

            var results = exp.Parse<Patient, PatientQuery>();

            Assert.AreEqual(1, results.Count());

            Assert.AreEqual("managingOrganization.reference", (results[0] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.Equal, (results[0] as SimpleExpression).Op);
            Assert.AreEqual("ID001", (results[0] as SimpleExpression).ExpectedValue);
        }

        [TestMethod]
        public void CanParseCompound()
        {
            Expression<Func<PatientQuery, bool>> exp = x => x.Organization == "ID001" && x.LastUpdated < new DateTime(2024, 05, 03);// && x.Id == "1";

            var results = exp.Parse<Patient, PatientQuery>();

            Assert.AreEqual(1, results.Count());
            var compound = results[0] as ExpressionGroup;
            Assert.IsNotNull(compound);

            Assert.AreEqual(2, compound.Expressions.Count);
            Assert.AreEqual(Lumbre.Interfaces.Query.LogicalGroupOperand.And, compound.Op);

            Assert.AreEqual("managingOrganization.reference", (compound.Expressions[0] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.Equal, (compound.Expressions[0] as SimpleExpression).Op);
            Assert.AreEqual("ID001", (compound.Expressions[0] as SimpleExpression).ExpectedValue);

            Assert.AreEqual("meta.lastUpdated", (compound.Expressions[1] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.LessThan, (compound.Expressions[1] as SimpleExpression).Op);
            Assert.AreEqual(new DateTime(2024, 05, 03), (compound.Expressions[1] as SimpleExpression).ExpectedValue);
        }


        [TestMethod]
        public void CanParseGroupings()
        {
            Expression<Func<PatientQuery, bool>> exp = x => x.Organization == "ID001" && (x.LastUpdated < new DateTime(2024, 05, 03) || x.Id == "1");

            var results = exp.Parse<Patient, PatientQuery>();

            Assert.AreEqual(2, results.Count());
            var compound = results[0] as ExpressionGroup;
            Assert.IsNotNull(compound);

            Assert.AreEqual(2, compound.Expressions.Count);
            Assert.AreEqual(Lumbre.Interfaces.Query.LogicalGroupOperand.Or, compound.Op);


            Assert.AreEqual("meta.lastUpdated", (compound.Expressions[0] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.LessThan, (compound.Expressions[0] as SimpleExpression).Op);
            Assert.AreEqual(new DateTime(2024, 05, 03), (compound.Expressions[0] as SimpleExpression).ExpectedValue);

            Assert.AreEqual("id", (compound.Expressions[1] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.Equal, (compound.Expressions[1] as SimpleExpression).Op);
            Assert.AreEqual("1", (compound.Expressions[1] as SimpleExpression).ExpectedValue);


            compound = results[1] as ExpressionGroup;
            Assert.IsNotNull(compound);

            Assert.AreEqual(1, compound.Expressions.Count);
            Assert.AreEqual(Lumbre.Interfaces.Query.LogicalGroupOperand.And, compound.Op);

            Assert.AreEqual("managingOrganization.reference", (compound.Expressions[0] as SimpleExpression).Path);
            Assert.AreEqual(ExpressionType.Equal, (compound.Expressions[0] as SimpleExpression).Op);
            Assert.AreEqual("ID001", (compound.Expressions[0] as SimpleExpression).ExpectedValue);
        }
    }
}
