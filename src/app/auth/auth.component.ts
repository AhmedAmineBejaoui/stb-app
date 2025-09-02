import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent {
  showLogin = true;
  loginForm: FormGroup;
  signupForm: FormGroup;
  isLoading = false;
  loginError = '';

  signupError = '';
  signupSuccess = false;
  loginSuccess = false;
  showLoginPassword = false;
  showSignupPassword = false;
  showSignupConfirmPassword = false;


  constructor(private fb: FormBuilder, private router: Router) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });

    this.signupForm = this.fb.group({
      prenom: ['', [Validators.required, Validators.minLength(2)]],
      nom: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      telephone: ['', [Validators.pattern(/^\+?\d{8,15}$/)]],
      motdepasse: ['', [Validators.required, Validators.minLength(8)]],
      confirmation: ['', [Validators.required, Validators.minLength(8)]],
      conditions: [false, Validators.requiredTrue]
    });
  }


  toggleShowLoginPassword() {
    this.showLoginPassword = !this.showLoginPassword;
  }

  toggleShowSignupPassword() {
    this.showSignupPassword = !this.showSignupPassword;
  }

  toggleShowSignupConfirmPassword() {
    this.showSignupConfirmPassword = !this.showSignupConfirmPassword;
  }

  toggleForm() {
    this.showLogin = !this.showLogin;
    this.loginError = '';
    this.signupError = '';
    this.signupSuccess = false;
    this.loginSuccess = false;
  }

  handleLogin() {
    this.loginError = '';
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    const { email, password } = this.loginForm.value;

    setTimeout(() => {

      const accountsJson = localStorage.getItem('accounts');
      const accounts = accountsJson ? JSON.parse(accountsJson) : [];
      const account = accounts.find((acc: any) => acc.email === email && acc.password === password);
      if (account) {
        localStorage.setItem('token', 'fake_token');
        localStorage.setItem('login', email);
        this.loginSuccess = true;
        setTimeout(() => {
          this.router.navigate(['/dashboard']);
          this.loginSuccess = false;
        }, 1500);

      } else {
        this.loginError = 'Email ou mot de passe incorrect';
      }
      this.isLoading = false;
    }, 1000);
  }

  handleSignup() {

    this.signupError = '';
    if (this.signupForm.invalid ||
        this.signupForm.value.motdepasse !== this.signupForm.value.confirmation) {
      this.signupForm.markAllAsTouched();
      return;
    }

    const accountsJson = localStorage.getItem('accounts');
    const accounts = accountsJson ? JSON.parse(accountsJson) : [];
    if (accounts.some((acc: any) => acc.email === this.signupForm.value.email)) {
      this.signupError = 'Un compte avec cet email existe déjà';
      return;
    }

    accounts.push({
      prenom: this.signupForm.value.prenom,
      nom: this.signupForm.value.nom,
      email: this.signupForm.value.email,
      telephone: this.signupForm.value.telephone,
      password: this.signupForm.value.motdepasse
    });
    localStorage.setItem('accounts', JSON.stringify(accounts));


    this.signupSuccess = true;
    setTimeout(() => {
      this.toggleForm();
      this.signupSuccess = false;
    }, 3000);
  }
}

