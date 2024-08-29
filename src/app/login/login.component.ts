import { Component } from '@angular/core';
import { ServiceService } from '../service.service'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  username: string = '';
  password: string = '';
  showPassword: boolean = false;

  constructor(private authService: ServiceService , private router: Router) {}

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.username && this.password) {
      this.authService.login(this.username, this.password).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera && response.oContenido.Cabecera.length > 0) {
            const user = response.oContenido.Cabecera[0];
            const nUsuario = user.nUsuario;
            const cUsuario = user.cUsuario;
            const cCorreo = user.cCorreo;
            const nRol = user.nRol;  // Obtener el rol del usuario

            // Almacenar la información del usuario en localStorage
            localStorage.setItem('nUsuario', nUsuario);
            localStorage.setItem('cUsuario', cUsuario);
            localStorage.setItem('cCorreo', cCorreo);

            // Redirigir según el rol
            switch (nRol) {
              case 1:
                this.router.navigate(['/administrador']);
                break;
              case 2:
                this.router.navigate(['/principal']);
                break;
              case 3:
                this.router.navigate(['/liderprincipal']);
                break;
              case 4:
                this.router.navigate(['/estudiante']);
                break;
              default:
                console.error('Rol no reconocido:', nRol);
                alert('Rol no reconocido');
                break;
            }
          } else {
            alert('Usuario o contraseña incorrectos');
          }
        },
        error => {
          console.error('Error en el login:', error);
        }
      );
    }
  }
}
