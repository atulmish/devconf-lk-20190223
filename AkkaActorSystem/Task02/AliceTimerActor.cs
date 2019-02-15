using System;
using Akka.Actor;
using Akka.Event;

namespace AkkaActorSystem.Task02
{
    public class AliceTimerActor:ReceiveActor
    {
        private int _sum;
        private ICancelable _cancelTimer;

        public AliceTimerActor(IActorRef destination)
        {
            var logger = Context.GetLogger();
            Receive<TimerMessages.AddValue>(a =>
            {
                logger.Info($"adding value: {a.Amount}");
                _sum += a.Amount;
                logger.Info($"Sum after operation: {_sum}");
            });

            Receive<TimerMessages.TimerTick>(timer =>
            {
                logger.Info($"Timer message received {DateTime.UtcNow}");
                destination.Tell(new TimerMessages.SummarisedValue(_sum));
                logger.Info($"Message sent to: {destination.Path}");
            });
        }

        protected override void PreStart()
        {
            _cancelTimer = new Cancelable(Context.System.Scheduler);

            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(5),
                Self,
                new TimerMessages.TimerTick(),
                Self,
                _cancelTimer);

            base.PreStart();
        }


    }
}
