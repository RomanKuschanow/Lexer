﻿using Lexer.Analyzer.IntermediateData;
using Lexer.Rules.RuleInputs.Interfaces;

namespace Lexer.Rules.RuleInputs;
public class CommonRuleInputCreator : IRuleInputCreator
{
    public IRuleInput Create(IntermediateDataCollection dataCollection)
    {
        ArgumentNullException.ThrowIfNull(dataCollection);

        string text = dataCollection.Get<InputTextIntermediateData>().GetData();

        return new CommonRuleInput(text);
    }
}
