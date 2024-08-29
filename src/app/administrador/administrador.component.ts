import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../service.service';

@Component({
  selector: 'app-administrador',
  templateUrl: './administrador.component.html',
  styleUrls: ['./administrador.component.css']
})
export class AdministradorComponent implements OnInit {

  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario

  display: boolean = false;
  showAlert: boolean = false;
  errorMessage: string = '';

  user = {
    nombre: '',
    correo: '',
    contrasena: '',
    bContraseniaSegura: false // Inicializamos el valor del checkbox
  };

  users: any[] = [];
  selectedProjects: string[] = [];

  constructor(private authService: ServiceService) {}

  ngOnInit(): void {
    // Obtener valores de localStorage
  const storedUsuario = localStorage.getItem('cUsuario');
  const storedCorreo = localStorage.getItem('cCorreo');

  // Asignar los valores a las propiedades del componente
  this.cUsuario = storedUsuario ? storedUsuario : 'Usuario no encontrado';
  this.cCorreo = storedCorreo ? storedCorreo : 'Correo no encontrado';

  // Cargar la lista de usuarios
  this.loadUsers();
  }

  loadUsers(): void {
    const storedUsuario = Number(localStorage.getItem('nUsuario'));

    this.authService.listarUsuarios(storedUsuario ? storedUsuario:0).subscribe(
      response => {
        console.log('Respuesta en loadUsers:', response);
        // Ajusta la asignación de datos según la estructura de la respuesta
        if (response && response.oContenido && response.oContenido.Cabecera) {
          this.users = response.oContenido.Cabecera;
        } else {
          this.handleError('Error al listar los usuarios: ' + (response.cMsjError || 'Respuesta inesperada'));
        }
      },
      error => {
        this.handleError('Error en el servicio: ' + error.message);
      }
    );
  }
  
  displayDialog(): void {
    this.display = true;
  }
  
  createUser(): void {
    this.authService.registro(this.user.nombre, this.user.correo, this.user.contrasena, this.user.bContraseniaSegura)
      .subscribe(
        response => {
          console.log('Respuesta en createUser:', response);
          if (response && response.nCodError === 0) {
            if (response.oContenido && response.oContenido.Cabecera && response.oContenido.Cabecera.length > 0) {
              this.resetForm();
              this.display = false;
              this.loadUsers();
            } else {
              this.handleError('El usuario se creó con éxito, pero no se devolvió información útil.');
            }
          } else {
            this.handleError('Error al crear el usuario: ' + (response.cMsjError || 'Error desconocido'));
          }
        },
        error => {
          this.handleError('Error en el servicio: ' + error.message);
        }
      );
  }


  
  

  fetchProjects(user: any): void {
    this.selectedProjects = user.proyectos;
    this.showAlert = this.selectedProjects.length === 0;
  }

  resetForm(): void {
    this.user = {
      nombre: '',
      correo: '',
      contrasena: '',
      bContraseniaSegura: false // Inicializamos el valor del checkbox
    };
  }

  handleError(message: string): void {
    console.error(message);
    this.errorMessage = message;
  }
}
