using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using SIC_Simulator.Extensions;
using static System.Windows.Forms.ListViewItem;
using System.Diagnostics;
using SIC_Simulator;

namespace SICVM_UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        //Unit test to test function by Daniel
        [TestMethod]
        public void RelocateLoadObjectFileTests()
        {
            var loader = new Form1();
            String[] lines = {
                "HCOPY  00100000107D",
                "T0010001E1410334820390010362810303010154820643C100300102A0C103900102D",
                "T00101E150C10364820640810334C0000454F46000003000000",
                "T0020391E041030001030E0205D30203FD8205D2810303020575490392C205E38203F",
                "T0020570B1010364C0000F100100053",
                "T00206419041030E0207C302067509039DC207C2C10363820674C000005",
                "E001000"
            };
            String[] mod =
            {
                "M00100104+COPY",
                "M00100404+COPY",
                "M00100704+COPY",
                "M00100A04+COPY",
                "M00100D04+COPY",
                "M00101004+COPY",
                "M00101304+COPY",
                "M00101604+COPY",
                "M00101904+COPY",
                "M00101F04+COPY",
                "M00102204+COPY",
                "M00102504+COPY",
                "M00203A04+COPY",
                "M00203D04+COPY",
                "M00204004+COPY",
                "M00204304+COPY",
                "M00204604+COPY",
                "M00204904+COPY",
                "M00204C04+COPY",
                "M00204F04+COPY",
                "M00205204+COPY",
                "M00205504+COPY",
                "M00205804+COPY",
                "M00206504+COPY",
                "M00206804+COPY",
                "M00206B04+COPY",
                "M00206E04+COPY",
                "M00207104+COPY",
                "M00207404+COPY",
                "M00207704+COPY"
            };

            // This is in the UNIT TEST
            //loader.RelocateLoadObjectFile(3000, lines, mod);

            // don't know what the output is supposed to look like but it outputs the array in debug trace
            // it appeares it's formatted as
            // original line
            // modified line
            // original line
            // modified line
            // .
            // .
            // .

        }
    }
}
