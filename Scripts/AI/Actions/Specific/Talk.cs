using Pearl.Debug;

namespace Pearl.AI
{
    public class Talk : AIAction
    {
        private string name;

        public Talk(string name)
        {
            this.name = name;
        }

        public override void Execute()
        {
            LogManager.Log("hi " + name);
        }
    }
}
