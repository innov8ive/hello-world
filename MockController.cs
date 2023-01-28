using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MockController : ApiController
    {
        private string path = "JSON/";

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/Login")]
        [HttpPost]
        public LoginResponse Login(LoginRequest loginRequest)
        {
            if (loginRequest.Password.Length > 6)
            {
                return new LoginResponse() { userID = 1 };
            }
            else
            {
                return new LoginResponse() { userID = 0 };
            }
        }

        [Route("api/GetAllPatient")]
        [HttpPost]
        public List<GetPat> GetAllPatient(SearchReq req)
        {
            string data = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/") + path + "GetPat.json");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetPat>>(data);
            return result.Where(Obj => string.IsNullOrEmpty(req.SearchName) || Obj.patientName.ToString().ToLower().Contains(req.SearchName.ToLower())).Skip((req.PageNo-1) * req.PageSize).Take(req.PageSize).ToList();
        }

        [Route("api/GetAllNursingImage")]
        [HttpGet]
        public List<GetNurImg> GetAllNursingImage()
        {
            string data = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/") + path + "GetNurImgs.json");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetNurImg>>(data);
            return result;
        }

        [Route("api/GetNursingDetails/{id}/{type}")]
        [HttpGet]
        public List<GetSavedImg> GetNursingDetails(int id, string type)
        {
            string data = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/") + path + "GetSavedImg.json");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetSavedImg>>(data);
            return result;
        }

        [Route("api/GetAllDoctorStump")]
        [HttpGet]
        public List<DocList> GetAllDoctorStump()
        {
            List<DocList> list = new List<DocList>();
            list.Add(new DocList() { doctorFName = "Test", doctorLName = "2", doctorType = "Note", uploadSignature = "img/1.png" });
            list.Add(new DocList() { doctorFName = "Testing", doctorLName = "1", doctorType = "Note", uploadSignature = "img/2.png" });
            return list;
        }

        [Route("api/IUNursingSheet")]
        [HttpPost]
        public string IUNursingSheet(List<IUNursingSheetReq> req)
        {
            return "success";
        }
        [Route("api/GetFileDetails")]
        [HttpPost]
        public string GetFileDetails(FileReq req)
        {
            string path = HttpContext.Current.Server.MapPath("~/") + req.FilePath;
            if (req.FileType == "image")
            {
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            else
            {
                return File.ReadAllText(path);
            }
        }

        [Route("api/EmailSend")]
        [HttpPost]
        public void EmailSend(EmailReq req)
        {
            
        }

        [Route("api/GeneratePaymentLink")]
        [HttpPost]
        public string GeneratePaymentLink(GenerateLinkReq req)
        {
            SetuHelper setuHelper = new SetuHelper();
            return setuHelper.GenerateDeepLink(req.Amount, "Test Soc", req.Note);
        }

        [Route("api/CheckLinkStatus/{refNo}")]
        [HttpGet]
        public string CheckLinkStatus(string refNo)
        {
            SetuHelper setuHelper = new SetuHelper();
            return setuHelper.CheckLinkStatus(refNo);
        }

        [Route("api/GetToken")]
        [HttpGet]
        public string GetToken()
        {
            return Convert.ToString(HttpContext.Current.Session["Token"]);
        }
    }
}
