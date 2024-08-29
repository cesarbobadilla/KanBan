using MACRO.Entidad.EntidadesDTO;
using MACRO.Servicio;
using SCOM.WebApi.Authorization.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Security.Principal;

namespace SCOM.WebApi.Authorization
{
    public class TokenAuthenticate : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            Usuario userRpt = null;
            try
            {
                var anonActionAttributes = context.ActionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
                var anonControllerAttributes = context.ActionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>();
                if (anonActionAttributes.Count > 0 || anonControllerAttributes.Count > 0)
                    return;

                IAuthService _authService = context.ActionContext.ControllerContext.Configuration
                                  .DependencyResolver.GetService(typeof(IAuthService)) as IAuthService;

                HttpRequestMessage request = context.Request;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;
                if (authorization == null)
                {
                    HttpCookie authCookie = HttpContext.Current.Request.Cookies[MENSAJE_REPT.COOKIE_SGD];
                    if (authCookie != null)
                        authorization = new AuthenticationHeaderValue("bearer", authCookie.Value);
                    else if (HttpContext.Current.Request.Headers[MENSAJE_REPT.COOKIE_SGD] != null) authorization = new AuthenticationHeaderValue("bearer", HttpContext.Current.Request.Headers[MENSAJE_REPT.COOKIE_SGD]);
                }
                if (authorization == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing autorization header", request);
                    return;
                }
                if (!authorization.Scheme.Equals("bearer", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid autorization scheme", request);
                    return;
                }
                if (String.IsNullOrEmpty(authorization.Parameter))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                    return;
                }

                userRpt = await _authService.ValidateTokenAsync(authorization.Parameter);
                /*if (!userRpt.cDominio.Equals("AQP"))
                {
                    context.ErrorResult = new AuthenticationFailureResult(userRpt.cDominio, request);
                    return;
                }*/

                /*var identity = new GenericIdentity(userRpt.cUsuario,userRpt.cPassword);
                context.Principal = new GenericPrincipal(identity, null);*/
            }
            catch (Exception ex)
            {
                var lnMensajeExcepcion = ex.Message.Replace("\n", " ");
                context.ErrorResult = new AuthenticationFailureResult("Exception: " + lnMensajeExcepcion, context.Request);
            }
        }
       
        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            await Task.FromResult(0);
            return;
        }
    }
}