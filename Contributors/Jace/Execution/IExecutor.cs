﻿using Jace.Operations;
using System;
using System.Collections.Generic;

namespace Jace.Execution
{
    public interface IExecutor
    {
        double Execute(Operation operation, IFunctionRegistry functionRegistry, IConstantRegistry constantRegistry);
        double Execute(Operation operation, IFunctionRegistry functionRegistry, IConstantRegistry constantRegistry, IDictionary<string, double> variables);

        Func<IDictionary<string, double>, double> BuildFormula(Operation operation, IFunctionRegistry functionRegistry, IConstantRegistry constantRegistry);
    }
}
