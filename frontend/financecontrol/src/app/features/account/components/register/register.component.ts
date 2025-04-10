import { Component, OnInit, AfterViewInit, ElementRef, ViewChildren } from '@angular/core';
import { FormBuilder, FormGroup, FormControlName, Validators } from '@angular/forms';
import { Observable, fromEvent, merge } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../models/user';
import { AccountService } from '../../services/account.service';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/core/validators/generic-form-validation';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, AfterViewInit {

  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  registerForm!: FormGroup;
  user!: User;
  errors: any[] = [];
  
  validationMessages: ValidationMessages;
  genericValidator: GenericValidator;
  displayMessage: DisplayMessage = {};
  
  changesSaved: boolean = true;
  showPassword: boolean = false;
  returnUrl: String = '';  

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private toastr: ToastrService,
    private router: Router,
    private route : ActivatedRoute
  ) {
    this.validationMessages = {
      fullName: {
        required: 'Informe o nome',
      },
      email: {
        required: 'Informe o e-mail',
        email: 'E-mail inválido'
      },
      password: {
        required: 'Informe a senha',
        minlength: 'A senha deve ter pelo menos 6 caracteres.',
        maxlength: 'A senha não pode exceder 15 caracteres.'
      }
    };

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'];
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    });
  }

  ngAfterViewInit(): void {
    const controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.registerForm);
      this.changesSaved = false;
    });
  }

  registerUser(): void {
    if (this.registerForm.dirty && this.registerForm.valid) {
      this.user = Object.assign({}, this.user, this.registerForm.value);

      this.accountService.registerUser(this.user).subscribe({
        next: (success) => this.processSuccess(success),
        error: (error) => this.processFail(error)
      });

      this.changesSaved = true;
    }
  }

  processSuccess(response: any): void {
    this.registerForm.reset();
    this.errors = [];

    this.accountService.LocalStorage.saveLocalUserData(response);

    const toast = this.toastr.success('Cadastro realizado com sucesso!', 'Bem-vindo!');

    if (toast) {
      toast.onHidden.subscribe(() => {
        this.returnUrl ? this.router.navigate([this.returnUrl]) : this.router.navigate(['/home']);
      });
    }
  }

  processFail(fail: any): void {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
  
}
