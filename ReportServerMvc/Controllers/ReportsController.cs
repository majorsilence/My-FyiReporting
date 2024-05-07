using fyiReporting.RdlAsp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fyiReporting.ReportServerMvc.Controllers
{
    [Authorize(Policy = "Authed")]
    public class ReportsController : Controller
    {
        readonly private RdlReport _report;
        public ReportsController(RdlReport report)
        {
            _report = report;
        }

        // GET: ReportsController
        public ActionResult Index()
        {
            return View();
        }

        [ServiceFilter(typeof(HasReportPermissionsAttribute))]
        public ActionResult ViewerPdf()
        {

            //GJL 20080520 - Show report parameters without running it first (many line changes in this file)
            bool error;

            string sessionValue = HttpContext.Session.GetString("CurrentPdfReport");
            ReportSession ses = System.Text.Json.JsonSerializer.Deserialize<ReportSession>(sessionValue);

            string Name = ses.Name;
            bool FirstRun = ses.FirstRun;

            if (FirstRun)
            {
                _report.NoShow = true;
            }
            else
            {
                _report.NoShow = false;
            }


            _report.RenderType = "pdf";

            _report.PassPhrase = "northwind";       // user should provide in some fashion (from web.config??)
            // ReportFile must be the last item set since it triggers the building of the report

            string arg = ses.url;
            if (arg != null)
            {
                _report.ReportFile = arg;
            }

            if (_report.Object == null)
            {
                error = true;
            }
            else
            {
                //context.Response.ContentType = "application/pdf";

                context.Response.AddHeader("content-disposition", "inline; filename=myFyiReportingReport.pdf");
                context.Response.BinaryWrite(_report.Object);
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
