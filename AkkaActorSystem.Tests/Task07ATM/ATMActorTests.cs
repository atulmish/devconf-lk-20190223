using System;
using Akka.Actor;
using AkkaActorSystem.Task06;
using AkkaActorSystem.Task07ATM;
using Xunit;

namespace AkkaActorSystem.Tests.Task07ATM
{
    public class ATMActorTests : ActorTestBase
    {
        private int _expectedBalance;

        /*
         * atm rules
         * 1. can have notes of 200, 100, 50, 20
         * 2. amount from 40 to 1000
         * 3. if we are lack of notes, but still have other notes we shall dispense
         * 4. the ATM is feed with notes from time to time
         */
        [Fact]
        public void WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent()
        {
            // arrange - given
            var props = Props.Create(() => new ATMActor());
            _sut = Sys.ActorOf(props);
            _expectedBalance = 5 * 200 + 5 * 100 + 5 * 50 + 5 * 20;

            // act - when
            _sut.Tell(new ATMActor.FeedNotes(5, 5, 5, 5));

            // assert - then
            ExpectMsg<ATMActor.CashBalance>(m => { Assert.Equal(m.Balance, _expectedBalance); });
        }

        [Fact]
        public void WhenReceievNextNotesMsgThenBalanceShallBeUpdated()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            _expectedBalance += 20;

            // act - when
            _sut.Tell(new ATMActor.FeedNotes(0, 0, 0, 1));

            // assert - then
            ExpectMsg<ATMActor.CashBalance>(m => { Assert.Equal(_expectedBalance, m.Balance); });
        }

        [Fact]
        public void WhenAskForDisposeLessThan40ErrorMsgSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(30));

            // assert - then
            ExpectMsg<ATMActor.CannotDispense>(m => { Assert.Same("ToLow", m.Reason); });
        }


        [Fact]
        public void WhenAskForDisposeMoreThan1000ErrorMsgSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(1010));

            // assert - then
            ExpectMsg<ATMActor.CannotDispense>(m => { Assert.Same("ToHigh", m.Reason); });
        }


        [Fact]
        public void WhenAskForDisposeMoreThanBalanceErrorMsgSent()
        {
            // arrange - given
            var props = Props.Create(() => new ATMActor());
            _sut = Sys.ActorOf(props);
            _sut.Tell(new ATMActor.FeedNotes(1, 1, 1, 1));
            ExpectMsg<ATMActor.CashBalance>();

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(500));


            // assert - then
            ExpectMsg<ATMActor.CannotDispense>(m => { Assert.Same("Balance", m.Reason); });
        }

        [Fact]
        public void WhenAskFor140ThenProperNotesShallBeSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            /*
             * 100+20+20 => 1x100 + 2x20
             */

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(140));

            // assert - then
            ExpectMsg<ATMActor.DispensedNotes>(m =>
                {
                    Assert.Equal(0, m.N200);
                    Assert.Equal(1, m.N100);
                    Assert.Equal(0, m.N50);
                    Assert.Equal(2, m.N20);
                }
            );
        }


        [Fact]
        public void WhenAskFor170ThenProperNotesShallBeSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            /*
             * 100+50+20 => 1x100 + 1x50 +1x20
             */

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(170));

            // assert - then
            ExpectMsg<ATMActor.DispensedNotes>(m =>
                {
                    Assert.Equal(0, m.N200);
                    Assert.Equal(1, m.N100);
                    Assert.Equal(1, m.N50);
                    Assert.Equal(1, m.N20);
                }
            );
        }

        [Fact]
        public void WhenAskFor110ThenProperNotesShallBeSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            /*
             * 100 + 10 => 1x50 + 3x20
             */

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(110));

            // assert - then
            ExpectMsg<ATMActor.DispensedNotes>(m =>
                {
                    Assert.Equal(0, m.N200);
                    Assert.Equal(0, m.N100);
                    Assert.Equal(1, m.N50);
                    Assert.Equal(3, m.N20);
                }
            );
        }


        [Fact]
        public void WhenAskFor500MultipleTimesThenProperNotesShallBeSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(500));
            _sut.Tell(new ATMActor.GiveMeMyCash(500));
            _sut.Tell(new ATMActor.GiveMeMyCash(500));

            // assert - then
            ReceiveN(2);
            ExpectMsg<ATMActor.DispensedNotes>(m =>
                {
                    Assert.Equal(1, m.N200);
                    Assert.Equal(3, m.N100);
                    Assert.Equal(0, m.N50);
                    Assert.Equal(0, m.N20);
                }
            );


        }









        [Fact]
        public void WhenAskFor90ThenProperNotesShallBeSent()
        {
            // arrange - given
            WhenGetSeedNotesThenConfirmationWithSumOfCashShallBeSent();
            /*
             * 50 + 2 x 20
             */

            // act - when
            _sut.Tell(new ATMActor.GiveMeMyCash(90));

            // assert - then
            ExpectMsg<ATMActor.DispensedNotes>(m =>
                {
                    Assert.Equal(0, m.N200);
                    Assert.Equal(0, m.N100);
                    Assert.Equal(1, m.N50);
                    Assert.Equal(2, m.N20);
                }
            );
        }
//1850

    }
}
