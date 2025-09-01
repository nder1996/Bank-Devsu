import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class LoggingInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Log de la petición saliente
    this.logRequest(req);

    return next.handle(req).pipe(
      tap({
        next: (event) => {
          if (event instanceof HttpResponse) {
            this.logResponse(req, event);
          }
        },
        error: (error: HttpErrorResponse) => {
          this.logError(req, error);
        }
      })
    );
  }

  private logRequest(req: HttpRequest<any>): void {
    const requestInfo = {
      method: req.method,
      url: req.url,
      headers: this.extractHeaders(req.headers),
      body: req.body,
      params: this.extractParams(req.params)
    };

    //console.log('🚀 HTTP REQUEST:', JSON.stringify(requestInfo, null, 2));

    // También log la URL completa para copiar fácilmente en Postman
    const fullUrl = req.params.keys().length > 0
      ? `${req.url}?${req.params.toString()}`
      : req.url;

    //console.log('🔗 POSTMAN URL:', `${req.method} ${fullUrl}`);

    if (req.body) {
      //console.log('📦 POSTMAN BODY:', JSON.stringify(req.body, null, 2));
    }
  }

  private logResponse(req: HttpRequest<any>, res: HttpResponse<any>): void {
    const responseInfo = {
      method: req.method,
      url: req.url,
      status: res.status,
      statusText: res.statusText,
      body: res.body,
      headers: this.extractHeaders(res.headers)
    };

    //console.log('✅ HTTP RESPONSE:', JSON.stringify(responseInfo, null, 2));
  }

  private logError(req: HttpRequest<any>, error: HttpErrorResponse): void {
    const errorInfo = {
      method: req.method,
      url: req.url,
      status: error.status,
      statusText: error.statusText,
      error: error.error,
      message: error.message
    };

    //console.error('❌ HTTP ERROR:', JSON.stringify(errorInfo, null, 2));
  }

  private extractHeaders(headers: any): any {
    const headerObj: any = {};
    if (headers && headers.keys) {
      headers.keys().forEach((key: string) => {
        headerObj[key] = headers.get(key);
      });
    }
    return headerObj;
  }

  private extractParams(params: any): any {
    const paramObj: any = {};
    if (params && params.keys) {
      params.keys().forEach((key: string) => {
        paramObj[key] = params.get(key);
      });
    }
    return paramObj;
  }
}
