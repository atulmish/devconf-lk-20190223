using System.Net.NetworkInformation;
using Akka.Actor;

namespace AkkaActorSystem.Task07ATM
{
    public class ATMActor : ReceiveActor
    {
        private int _n200;
        private int _n20;
        private int _n100;
        private int _n50;
        private int _cashBalance;
        private const int Twenty = 20;

        public ATMActor()
        {
            Receive<FeedNotes>(f =>
            {
                _n200 += f.N200;
                _n100 += f.N100;
                _n50 += f.N50;
                _n20 += f.N20;
                _cashBalance = _n200 * 200 + _n100 * 100 + _n50 * 50 + _n20 * 20;
                Sender.Tell(new CashBalance(_cashBalance));
            });

            Receive<GiveMeMyCash>(n =>
            {
                if (n.Amount < 40)
                {
                    Sender.Tell(new CannotDispense("ToLow"));
                    return;
                }

                if (n.Amount > 1000)
                {
                    Sender.Tell(new CannotDispense("ToHigh"));
                    return;
                }

                if (n.Amount > _cashBalance)
                {
                    Sender.Tell(new CannotDispense("Balance"));
                    return;
                }


                var n200 = 0;
                var n100 = 0;
                var n50 = 0;
                var n20 = 0;
                var removeOneFifty = false;
                var amount = n.Amount;


                var restDiv20 = amount % 20;
                if (restDiv20 == 10)
                {
                    // we have a  need to substitute a 10 note;
                    // do we need a 2 x 50 instead of 100
                    if (amount % 100 < 50)
                    {
                        // ok we need to change 100 into one 50 and process
                        amount -= 50;
                        n50 = 1;
                        removeOneFifty = true;
                    }
                }



                while (amount >= 200 && _n200>0)
                {
                    n200 += 1;
                    _n200--;
                    amount -= 200;
                }


                while (amount >= 100&& _n100>0)
                {
                    n100 += 1;
                    _n100--;
                    amount -= 100;
                }


                while (amount >= 50&& _n50>0)
                {
                    n50 += 1;
                    _n50--;
                    amount -= 50;
                }

                if (removeOneFifty)
                {
                    _n50++;
                    n50--;
                    amount += 50;
                }

                while (amount >= Twenty&& _n20>0)
                {
                    n20 += 1;
                    _n20--;
                    amount -= Twenty;
                }

                if (amount > 0)
                {
                    // we have problem here
                }
                ;


                Sender.Tell(new DispensedNotes(n200, n100, n50, n20));
            });
        }

        public class FeedNotes
        {
            public int N200 { get; }
            public int N100 { get; }
            public int N50 { get; }
            public int N20 { get; }

            public FeedNotes(int n200, int n100, int n50, int n20)
            {
                N200 = n200;
                N100 = n100;
                N50 = n50;
                N20 = n20;
            }
        }

        public class CashBalance
        {
            public int Balance { get; }

            public CashBalance(int balance)
            {
                Balance = balance;
            }
        }

        public class GiveMeMyCash
        {
            public int Amount { get; }

            public GiveMeMyCash(int amount)
            {
                Amount = amount;
            }
        }

        public class DispensedNotes : FeedNotes
        {
            public DispensedNotes(int n200, int n100, int n50, int n20) : base(n200, n100, n50, n20)
            {
            }
        }

        public class CannotDispense
        {
            public string Reason { get; }

            public CannotDispense(string reason)
            {
                Reason = reason;
            }
        }
    }
}
