﻿using Jace.Execution;
using System.Collections.Generic;

namespace Jace
{
    public class FormulaContext
    {
        public FormulaContext(IDictionary<string, double> variables,
            IFunctionRegistry functionRegistry,
            IConstantRegistry constantRegistry)
        {
            this.Variables = variables;
            this.FunctionRegistry = functionRegistry;
            this.ConstantRegistry = constantRegistry;
        }

        public IDictionary<string, double> Variables { get; private set; }

        public IFunctionRegistry FunctionRegistry { get; private set; }
        public IConstantRegistry ConstantRegistry { get; private set; }
    }
}
