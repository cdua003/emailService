using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eMailService.Exceptions;
using eMailService.Helps;
using eMailService.ViewModels;
using eMailService.Repository;
using System.Linq;

namespace eMailService.Tests
{
    [TestClass]
    public class ServiceTesting
    {
        [TestMethod]
        [ExpectedException(typeof(MissingTotalException), GlobalConstant.MISSING_TOTAL)]
        public void MissingTotal()
        {

            var value = @"Hi Yvaine,
                        Please create an expense claim for the below. Relevant details are marked up as
                        requested…
                        <expense><cost_centre>DEV002</cost_centre>
                        <payment_method>personal card</payment_method>
                        </expense>
                        From: Ivan Castle
                        Sent: Friday, 16 February 2018 10:32 AM
                        To: Antoine Lloyd <Antoine.Lloyd@example.com>
                        Subject: test
                        Hi Antoine,
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
                        <description>development team’s project end celebration dinner</description> on
                        <date>Tuesday 27 April 2017</date>. We expect to arrive around
                        7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        Regards,
                        Ivan";
            var emailViewModel = new EmailViewModel(new EmailRepository());
            emailViewModel.ProcessingData(StringToXml.ToXml(value));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidXmlException), GlobalConstant.INVALID_XML)]
        public void InvalidXml()
        {

            var value = @"Hi Yvaine,
                        Please create an expense claim for the below. Relevant details are marked up as
                        requested…
                        <expense><total>1024.01</total><cost_centre>DEV002</cost_centre>
                        <payment_method>personal card
                        </expense>
                        From: Ivan Castle
                        Sent: Friday, 16 February 2018 10:32 AM
                        To: Antoine Lloyd <Antoine.Lloyd@example.com>
                        Subject: test
                        Hi Antoine,
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
                        <description>development team’s project end celebration dinner</description> on
                        <date>Tuesday 27 April 2017</date>. We expect to arrive around
                        7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        Regards,
                        Ivan";
            StringToXml.ToXml(value);
        }

        [TestMethod]
        public void Missing_Cost_Centre()
        {
            var value = @"Hi Yvaine,
                        Please create an expense claim for the below. Relevant details are marked up as
                        requested…
                        <expense><total>1024.01</total>
                        <payment_method>personal card</payment_method>
                        </expense>
                        From: Ivan Castle
                        Sent: Friday, 16 February 2018 10:32 AM
                        To: Antoine Lloyd <Antoine.Lloyd@example.com>
                        Subject: test
                        Hi Antoine,
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
                        <description>development team’s project end celebration dinner</description> on
                        <date>Tuesday 27 April 2017</date>. We expect to arrive around
                        7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        Regards,
                        Ivan";
            var emailViewModel = new EmailViewModel(new EmailRepository());
            var xDocument = emailViewModel.ProcessingData(StringToXml.ToXml(value));
            var xCostCentre = xDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == GlobalConstant.COST_CENTRE);
            Assert.AreEqual(xCostCentre.Value, GlobalConstant.MISSING_COST_CENTRE);
        }

        [TestMethod]
        public void Gst_NetCost()
        {
            var value = @"Hi Yvaine,
                        Please create an expense claim for the below. Relevant details are marked up as
                        requested…
                        <expense><total>1024.01</total>
                        <payment_method>personal card</payment_method>
                        </expense>
                        From: Ivan Castle
                        Sent: Friday, 16 February 2018 10:32 AM
                        To: Antoine Lloyd <Antoine.Lloyd@example.com>
                        Subject: test
                        Hi Antoine,
                        Please create a reservation at the <vendor>Viaduct Steakhouse</vendor> our
                        <description>development team’s project end celebration dinner</description> on
                        <date>Tuesday 27 April 2017</date>. We expect to arrive around
                        7.15pm. Approximately 12 people but I’ll confirm exact numbers closer to the day.
                        Regards,
                        Ivan";
            var emailViewModel = new EmailViewModel(new EmailRepository());
            var xDocument = emailViewModel.ProcessingData(StringToXml.ToXml(value));
            var xGst = xDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == GlobalConstant.GST);
            var xNet_Total = xDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == GlobalConstant.NET_TOTAL);
            Assert.AreEqual(xGst.Value, "133.57");
            Assert.AreEqual(xNet_Total.Value, "890.44");
        }
    }
}
