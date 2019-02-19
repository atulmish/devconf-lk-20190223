namespace AkkaActorSystem.Task04
{
    public class Messages
    {
        public class UnsafeOperation {}
        public class GetStatus {}
        public class AddOne {}

        public class MyStatus{
            public int Value { get; }

            public MyStatus(int value)
            {
                Value = value;
            }
        }
    }
}
