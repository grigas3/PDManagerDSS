using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDManager.Core;

using PDManager.Core.Models;
using System.Threading.Tasks;
using PDManager.Core.Services;
using PDManager.Core.UnitTests;
using PDManager.Core.Common.Interfaces;
using PDManager.Core.Common.Extensions;
using PDManager.Core.Common.Testing;

namespace PDManager.JobRunner2.Tests.Controllers
{
    [TestClass]
    public class DataProxyTest
    {

        [TestMethod]
        public async Task TestSaveObservations()
        {

            IDataProxy proxy = new DataProxy(new DummyCredentialProvider());

            var list = new PDObservationCollection();
            list.Add(new PDObservation() { Value = 1, Timestamp = DateTime.Now.ToUnixTimestampMilli(), CodeId = "TEST", PatientId = "5900aa2a2f2cd563c4ae3027",Category="Motor" });

            var ret = await proxy.Insert(list);
            Assert.IsTrue(ret);

        }

        [TestMethod]
        public void TestObservations()
        {
          
            IDataProxy proxy = new DataProxy(new DummyCredentialProvider());
            var observations = proxy.Get<PDObservation>( 10, 0, "{patientid:\"5900aa2a2f2cd563c4ae3027\",deviceid:\"\",codeid:\"PDTFTS_MAX\",datefrom:0,dateto:0,aggr:\"total\"}", null).Result;
            Assert.IsTrue(observations.Count() ==1);

        }
        [TestMethod]
        public async Task TestTimeObservations()
        {
           

               IDataProxy proxy = new DataProxy(new DummyCredentialProvider());
            

            var observations = await proxy.Get<PDObservation>(  10, 0, "{patientid:\"5900aa2a2f2cd563c4ae3027\",deviceid:\"\",codeid:\"STBRADS30\",datefrom:0,dateto:0,aggr:\"time\"}", null);
            Assert.IsTrue(observations.Count()>0);

        }

        [TestMethod]
        public async Task TestLastMonthObservations()
        {
            IDataProxy proxy = new DataProxy(new DummyCredentialProvider());            
            var observations = await proxy.Get<PDObservation>(  10, 0, String.Format("{{patientid:\"{1}\",deviceid:\"\",codeid:\"PDTFTS_MAX\",datefrom:0,dateto:0,aggr:\"total\"}}", DateTime.Now.AddMonths(-1).ToUnixTimestamp(), "5900aa2a2f2cd563c4ae3027"), null);
            Assert.IsTrue(observations.Count() == 1);

        }



        [TestMethod]
        public void TestGetPatient()
        {
            IDataProxy proxy = new DataProxy(new DummyCredentialProvider());
          
            var patient = proxy.Get<PDPatient>("5762a1cd2f2cd5a244ca6855");
            Assert.IsTrue(patient != null);

        }

   



    }
}
