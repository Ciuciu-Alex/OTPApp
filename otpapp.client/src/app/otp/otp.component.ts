import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { OtpService } from '../services/otp.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OTPToken } from '../models/otpToken.model';

@Component({
  selector: 'app-otp',
  templateUrl: './otp.component.html',
  styleUrl: './otp.component.css'
})

export class OtpComponent {
  tokenFormControl = new FormControl('', [Validators.required, Validators.minLength(12)]);
  generatedTokens: string[] = [];
  expiryTime: Date = new Date();
  isLogged: boolean = false;

  constructor(
    private otpService: OtpService, 
    private snackBar: MatSnackBar) { }

  ngOnInit(): void {}

  generateToken(): void {
    this.otpService.generateToken().subscribe((data: OTPToken) => {
      if (!this.generatedTokens.includes(data.token)) {
        this.generatedTokens.push(data.token);
      }
      this.tokenFormControl.setValue(data.token);
      this.expiryTime = new Date(data.expiryTime);
    });
  }

  openSnackBar(message: string, duration: number): void {
    const snackBarRef = this.snackBar.open(message, "", {
      duration: duration,
    });

    snackBarRef.afterDismissed().subscribe(() => {
      this.isLogged = false;
    });
  }

  validateToken(): void {
    if (!this.tokenFormControl.valid || !this.generatedTokens.includes(this.tokenFormControl.value!)) {
      this.openSnackBar('Please enter a valid OTP.', 3000);
      return;
    }
      this.otpService.validateToken(this.tokenFormControl.value!, this.expiryTime).subscribe((data: OTPToken) => {
        this.isLogged = true;
        var minutes = new Date(data.expiryTime).getMinutes() - new Date().getMinutes();
        this.openSnackBar(`Succesfully logged in! Your OTP is valid ${minutes} minutes.`, minutes * 60000);
        
      }, (error) => {
        if (error.status === 400 && error.error === 'OTP expired or invalid.') {
          this.openSnackBar(`OTP validation failed: ${error.error}`, 5000);
          this.isLogged = false
        }
      });
  }

  back(): void {
    window.location.reload();
  }
}