namespace AkkaActorSystem.Task02
{
    public class TimerMessages
    {
        public class AddValue
        {
            public int Amount { get; }

            public AddValue(int amount)
            {
                Amount = amount;
            }
        }

        public class TimerTick
        {

        }

        public class SummarisedValue
        {
            public int Amount { get; }

            public SummarisedValue(int amount)
            {
                Amount = amount;
            }
        }
    }
}