using Rcm.Core;
using Rcm.Core.Enum;
using Rcm.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Rcm.Site.Controllers {
    public class AccountController : Controller {

        #region Fields

        private readonly UserService _userService;
        private readonly RoleService _roleService;

        #endregion

        #region Ctor

        public AccountController() {
            this._userService = new UserService();
            this._roleService = new RoleService();
        }

        #endregion

        #region Actions

        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult LogOut() {
            Session.RemoveAll();
            Response.Cookies.Clear();
            Request.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToRoute("HomePage");
        }

        public ActionResult GetCaptcha() {
            try {
                var code = CommonHelper.GenerateCode(5);
                var image = CommonHelper.CreateCaptcha(code);

                var hc = Request.Cookies[CommonHelper.CaptchaId];
                if(hc != null) {
                    hc.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(hc);
                }

                var captcha = CommonHelper.CreateHash(code.ToLowerInvariant(), CommonHelper.CaptchaSalt);
                var cookie = new HttpCookie(CommonHelper.CaptchaId, captcha);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return File(image, @"image/png");
            } catch(Exception exc) {
                return Content(exc.Message);
            }
        }

        public ActionResult GetQRCode() {
            try {
                var code = @"http://www.peims.net:8085/Account/Download?v=mobile";
                var image = CommonHelper.CreateQRCode(code);
                return File(image, @"image/png");
            } catch(Exception exc) {
                return Content(exc.Message);
            }
        }

        public ActionResult Download(string v) {
            try {
                if("mobile".Equals(v, StringComparison.CurrentCultureIgnoreCase)) {
                    var filePath = Server.MapPath(@"/Content/files/rcms.apk");
                    using(var fs = new FileStream(filePath, FileMode.Open)) {
                        var bytes = new byte[(Int32)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);

                        return File(bytes, @"application/octet-stream", string.Format("{0}.apk", Guid.NewGuid().ToString("N")));
                    }
                }

                throw new Exception("无效的参数");
            } catch(Exception exc) {
                return Content(exc.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string uid, string password, string captcha, string returnUrl) {
            try {
                if(String.IsNullOrWhiteSpace(uid))
                    ModelState.AddModelError("", "用户名不能为空。");

                uid = uid.Trim(); ViewBag.Uid = uid;

                if(ModelState.IsValid && String.IsNullOrWhiteSpace(password))
                    ModelState.AddModelError("", "密码不能为空。");

                if(ModelState.IsValid && String.IsNullOrWhiteSpace(captcha))
                    ModelState.AddModelError("", "验证码不能为空。");

                if(ModelState.IsValid && Request.Cookies[CommonHelper.CaptchaId] == null)
                    ModelState.AddModelError("", "您的浏览器禁用了JavaScript，启用后才能使用本系统。");

                if(ModelState.IsValid) {
                    var code = Request.Cookies[CommonHelper.CaptchaId].Value;
                    captcha = CommonHelper.CreateHash(captcha.ToLowerInvariant().Trim(), CommonHelper.CaptchaSalt);
                    if(captcha != code)
                        ModelState.AddModelError("", "验证码错误。");
                }

                if(ModelState.IsValid) {
                    var loginResult = _userService.Validate(uid, password);
                    switch(loginResult) {
                        case LoginResult.Successful:
                            break;
                        case LoginResult.NotExist:
                            ModelState.AddModelError("", "用户名不存在。");
                            break;
                        case LoginResult.NotEnabled:
                            ModelState.AddModelError("", "用户已禁用，请与管理员联系。");
                            break;
                        case LoginResult.Expired:
                            ModelState.AddModelError("", "用户已过期，请与管理员联系。");
                            break;
                        case LoginResult.WrongPassword:
                        default:
                            ModelState.AddModelError("", "密码错误，登录失败。");
                            break;
                    }
                }

                if(ModelState.IsValid) {
                    var current = _userService.GetUser(uid);
                    var loginResult = _roleService.Validate(current.GroupId);
                    switch(loginResult) {
                        case LoginResult.Successful:
                            break;
                        case LoginResult.RoleNotExist:
                            ModelState.AddModelError("", "角色不存在。");
                            break;
                        case LoginResult.RoleNotEnabled:
                            ModelState.AddModelError("", "角色已禁用，请与管理员联系。");
                            break;
                        default:
                            ModelState.AddModelError("", "角色错误。");
                            break;
                    }
                }

                if(ModelState.IsValid) {
                    var current = _userService.GetUser(uid);
                    var now = DateTime.Now;
                    var ticket = new FormsAuthenticationTicket(
                        1,
                        uid,
                        now,
                        now.Add(FormsAuthentication.Timeout),
                        false,
                        current.LastId == 0 ? "10078" : current.GroupId.ToString(),
                        FormsAuthentication.FormsCookiePath);

                    var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    authCookie.HttpOnly = true;
                    authCookie.Path = FormsAuthentication.FormsCookiePath;

                    if(ticket.IsPersistent) {
                        authCookie.Expires = ticket.Expiration;
                    }

                    if(FormsAuthentication.CookieDomain != null) {
                        authCookie.Domain = FormsAuthentication.CookieDomain;
                    }

                    Response.Cookies.Add(authCookie);
                    if(Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToRoute("HomePage");
                }
            } catch(Exception exc) {
                ModelState.AddModelError("", exc.Message);
            }

            return View();
        }

        #endregion

    }
}