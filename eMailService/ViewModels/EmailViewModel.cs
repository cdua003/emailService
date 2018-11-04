using System;
using System.Net;
using System.Web.Http;
using System.Xml.Linq;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;
using eMailService.Results;
using eMailService.IRepository;
using eMailService.Models;
using eMailService.Helps;
using eMailService.Exceptions;

namespace eMailService.ViewModels
{
    public class EmailViewModel
    {
        private IEmailRepository _emailRepository;

        public EmailViewModel(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var list = new List<dynamic>();
                var records = _emailRepository.Get();
                foreach(var record in records)
                {
                    var xDocument = ProcessingData(record.Data);
                    dynamic root = new ExpandoObject();
                    XmlToDynamic.Parse(root, xDocument.Elements().First());
                    list.Add(root.root);
                }
                return new CustomResponse(HttpStatusCode.OK, list);
            }
            catch (Exception ex)
            {
                return new CustomResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        public IHttpActionResult Get(Guid Id)
        {
            try
            {
                var record = _emailRepository.Get(Id);
                if(record == null)
                {
                    return new CustomResponse(HttpStatusCode.OK, null);
                }
                var xDocument = ProcessingData(record.Data);
                dynamic root = new ExpandoObject();
                XmlToDynamic.Parse(root, xDocument.Elements().First());
                return new CustomResponse(HttpStatusCode.OK, root.root);
            }
            catch (Exception ex)
            {
                return new CustomResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult Delete(Guid Id)
        {
            try
            {
                _emailRepository.Delete(Id);
                return new CustomResponse(HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new CustomResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public IHttpActionResult Create(string value)
        {
            try
            {
                var xml = StringToXml.ToXml(value);
                var root = GetResult(xml);

                _emailRepository.Create(new EmailRecord { Data = xml });
                return new CustomResponse(HttpStatusCode.OK, root.root);
            }
            catch (Exception ex)
            {
                return new CustomResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public dynamic GetResult(string xml)
        {
            var xDocument = ProcessingData(xml);

            dynamic root = new ExpandoObject();
            XmlToDynamic.Parse(root, xDocument.Elements().First());
            return root;
        }

        public XDocument ProcessingData(string xml)
        {
            var xDocument = XDocument.Parse(xml);
            var xTotal = xDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == GlobalConstant.TOTAL);
            if (xTotal == null)
            {
                throw new MissingTotalException(GlobalConstant.MISSING_TOTAL);
            }
            var net_total = GetNetTotal(Convert.ToDouble(xTotal.Value));

            var parent = xTotal.Parent;
            parent.Add(new XElement(GlobalConstant.NET_TOTAL, net_total));
            parent.Add(new XElement(GlobalConstant.GST, GetTax(net_total)));

            var xCostCentre = xDocument.Descendants().SingleOrDefault(p => p.Name.LocalName == GlobalConstant.COST_CENTRE);
            if(xCostCentre != null)
            {
                return xDocument;
            }

            parent.Add(new XElement(GlobalConstant.COST_CENTRE, GlobalConstant.MISSING_COST_CENTRE));

            return xDocument;
        }

        private decimal GetNetTotal(double total)
        {
            return decimal.Round(decimal.Parse((total / (1 + GlobalConstant.TAX_RATE)).ToString()), 2);
        }

        private decimal GetTax(decimal netTotal)
        {
            return decimal.Round(decimal.Parse((netTotal * Convert.ToDecimal(GlobalConstant.TAX_RATE)).ToString()), 2);
        }
    }
}