﻿// Copyright (c) 2018 Javier Cañon 
// https://www.javiercanon.com 
// https://www.xn--javiercaon-09a.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using System;
using System.Web;
using Errors;

namespace Errors
{
    public partial class ErrorPageHttp : System.Web.UI.Page
    {
        protected HttpException ex = null;
        protected void Page_Load( object sender, EventArgs e )
        {

            // if System.InvalidOperationException show error:
            // use class AS class for trycast that return null if trycast fail...
            // http://stackoverflow.com/questions/3350770/how-to-convert-trycast-in-c 
            HttpException ex = Server.GetLastError() as HttpException;

            if (ex == null)
                return;

            int httpCode = ex.GetHttpCode();

            // log original error
            // logged in SO.Global.Application_Error():
            // ExceptionUtility.LogException( ex, "HttpErrorPage" );
            // ExceptionUtility.NotifySystemOps( ex );

            exTrace.Text = ex.StackTrace;


            if (Request.IsLocal || Pagame.Global.Configuration.Development.GetIsEnabledDeveloperMode())
            {

                if (ex.InnerException != null)
                {
                    innerTrace.Text = ex.InnerException.StackTrace;
                    InnerErrorPanel.Visible = Request.IsLocal;
                    innerMessage.Text = string.Format( "HTTP {0}: {1}",
                        httpCode, ex.InnerException.Message );
                }
                exTrace.Visible = Request.IsLocal;

            }


            /*
            if (httpCode >= 400 && httpCode < 500)
            {
                ex = new HttpException
                        ( httpCode, "Error al solicitar el recurso al servidor (" + httpCode + "). El error ha sigo registrado y notificado a los administradores.", ex );
            }
            else
            {
                if (httpCode > 499)
                {
                    ex = new HttpException
                        ( ex.ErrorCode, "Error en el aplicativo web del servidor (" + httpCode + "). El error ha sigo registrado y notificado a los administradores.", ex );
                }
                else
                {
                    ex = new HttpException
                        ( httpCode, "Safe message for unexpected HTTP codes.", ex );
                }
            }

                        exMessage.Text = ex.Message;

            */
            if (httpCode >= 400 && httpCode < 500)
            {
                exMessage.Text = 
                        "Error al solicitar el recurso al servidor (" + httpCode + "). El error ha sigo registrado y notificado a los administradores.";
            }
            else
            {
                if (httpCode > 499)
                {
                    exMessage.Text =
                        "Error en el aplicativo web del servidor (" + httpCode + "). El error ha sigo registrado y notificado a los administradores.";
                }
                else
                {
                    exMessage.Text = "Error no controlado en el aplicativo web del servidor (" + httpCode + "). El error ha sigo registrado y notificado a los administradores.";
                }
            }




            Server.ClearError();
        }
    }
}
