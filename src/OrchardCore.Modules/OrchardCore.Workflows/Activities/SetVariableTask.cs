using System.Collections.Generic;
using Microsoft.Extensions.Localization;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;

namespace OrchardCore.Workflows.Activities
{
    public class SetVariableTask : TaskActivity
    {
        public SetVariableTask(IStringLocalizer<SetVariableTask> localizer)
        {
            T = localizer;
        }

        private IStringLocalizer T { get; }

        public override string Name => nameof(SetVariableTask);
        public override LocalizedString Category => T["Primitives"];
        public override LocalizedString Description => T["Assigns a value to a variable on the workflow."];

        public string VariableName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public WorkflowExpression<object> Expression
        {
            get => GetProperty(() => new WorkflowExpression<object>());
            set => SetProperty(value);
        }

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(T["Done"]);
        }

        public override IEnumerable<string> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            var value = workflowContext.Evaluate(Expression);
            workflowContext.Variables[VariableName] = value;

            yield return "Done";
        }
    }
}