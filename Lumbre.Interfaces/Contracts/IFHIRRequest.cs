﻿using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumbre.Interfaces.Contracts;

public interface IFHIRRequest
{
    IIdentifiable<List<Identifier>> Entity { get; }
}