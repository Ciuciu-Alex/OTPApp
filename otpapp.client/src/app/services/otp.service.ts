import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OtpService {
  private baseUrl = 'https://localhost:7060/otp';

  constructor(private http: HttpClient) { }

  generateToken(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}`);
  }

  validateToken(token: string, expiryTime: Date): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/validate`, { Token: token, ExpiryTime: expiryTime });
  }
}
