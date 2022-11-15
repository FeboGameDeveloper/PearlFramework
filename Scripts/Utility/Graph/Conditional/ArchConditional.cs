namespace Pearl.Graph
{
    public class ArchConditional<ConditionalType> : Arch
    {
        private ConditionalType condition;
        private bool noCondition;

        public ArchConditional(in int weight) : base(weight)
        {
            noCondition = true;
        }

        public ArchConditional(in int weight, in ConditionalType condition) : base(weight)
        {
            SetNewCondition(condition);
        }

        public bool IsRespectCondition(in ConditionalType currentCondition)
        {
            return noCondition ? true : condition.Equals(currentCondition);
        }

        public void SetNewCondition(in ConditionalType condition)
        {
            if (condition != null)
            {
                noCondition = false;
                this.condition = condition;
            }
            else
            {
                noCondition = true;
            }
        }

    }
}
