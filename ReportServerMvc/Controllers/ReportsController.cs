using fyiReporting.RdlAsp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fyiReporting.ReportServerMvc.Controllers
{
    public class ReportsController : Controller
    {
        // GET: ReportsController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewerPdf()
        {


            RdlReport _Report;

            //GJL 20080520 - Show report parameters without running it first (many line changes in this file)
            bool error;

            _Report = new RdlReport();

            ReportSession ses = (ReportSession)context.Session["CurrentPdfReport"];


            string Name = ses.Name;
            bool FirstRun = ses.FirstRun;

            if (Security.HasPermissions(ses.url) == false)
            {
                return;
            }

            if (FirstRun)
            {
                _Report.NoShow = true;
            }
            else
            {
                _Report.NoShow = false;
            }


            _Report.RenderType = "pdf";

            _Report.PassPhrase = "northwind";       // user should provide in some fashion (from web.config??)
            // ReportFile must be the last item set since it triggers the building of the report

            string arg = ses.url;
            if (arg != null)
            {
                _Report.ReportFile = arg;
            }

            if (_Report.Object == null)
            {
                error = true;
            }
            else
            {
                //context.Response.ContentType = "application/pdf";

                context.Response.AddHeader("content-disposition", "inline; filename=myFyiReportingReport.pdf");
                context.Response.BinaryWrite(_Report.Object);
            }
            return View();
        }

        // GET: ReportsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReportsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReportsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
