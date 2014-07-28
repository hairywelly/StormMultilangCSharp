using NSubstitute;
using NUnit.Framework;
using StormMultiLang;
using StormMultiLang.Write;

namespace StormMultiLangTests.Write
{
    [TestFixture]
    public class GivenAThreadSafeBoltWriter
    {
        [Test]
        public void ShouldCallAcknowledge()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);
            
            subjectUndertest.Acknowledge(2);
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().Acknowledge(2);
        }

        [Test]
        public void ShouldCallFail()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);

            subjectUndertest.Fail(2);
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().Fail(2);
        }

        [Test]
        public void ShouldCallLogInfo()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);

            subjectUndertest.LogInfo("hello");
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().LogInfo("hello");
        }

        [Test]
        public void ShouldCallLogError()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);

            subjectUndertest.LogError("Error");
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().LogError("Error");
        }

        [Test]
        public void ShouldCallEmitTuple()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);

            subjectUndertest.EmitTuple(null,null,null);
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().EmitTuple(null, null, null);
        }

        [Test]
        public void ShouldCallEmitTupleDirect()
        {
            var wrappedBoltWriter = Substitute.For<IBoltWriter>();
            var subjectUndertest = new ThreadSafeBoltWriter(wrappedBoltWriter);

            subjectUndertest.EmitTupleDirect(null, null, 5, null);
            subjectUndertest.Shutdown();

            wrappedBoltWriter.Received().EmitTupleDirect(null, null, 5, null);
        }
    }
}