import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const token = localStorage.getItem('token');

      if(token) {
        const cloneRequest = req.clone({
          headers: req.headers.set('Authorization', `bearer ${token}`)
        })
        return next.handle(cloneRequest);
      }

      return next.handle(req);
  }
}
