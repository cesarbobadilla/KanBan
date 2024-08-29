import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../service.service';

@Component({
  selector: 'app-gestor',
  templateUrl: './gestor.component.html',
  styleUrls: ['./gestor.component.css']
})
export class GestorComponent implements OnInit {

  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario

  display: boolean = false;
  showAlert: boolean = false;
  errorMessage: string = '';

  user = {
    nombre: '',
    correo: '',
    contrasena: '',
    carrera: 0, // Inicializado como 0, debería ser un número
    ciclo: 0, // Inicializado como 0
    clase: '',
    bContraseniaSegura: false // Inicializamos el valor del checkbox
  };

  users: any[] = [];
  selectedProjects: string[] = [];
  carreras: any[] = [];

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
    this.loadCarreras();
  }

  loadCarreras(): void {
    this.authService.listarCarreras().subscribe(
      response => {
        if (response && response.oContenido && response.oContenido.Cabecera) {
          this.carreras = response.oContenido.Cabecera; // Ajusta esto según la estructura de tu respuesta
        } else {
          console.error('Error al obtener las carreras:', response.cMsjError || 'Respuesta inesperada');
        }
      },
      error => {
        console.error('Error en el servicio:', error.message);
      }
    );
  }

  loadUsers(): void {
    const storedUsuario = Number(localStorage.getItem('nUsuario'));

    this.authService.listarUsuarios(storedUsuario).subscribe(
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
    this.authService.registroalumnos(this.user.nombre, this.user.correo, this.user.contrasena, this.user.carrera, Number(this.user.ciclo), this.user.clase, this.user.bContraseniaSegura)
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

  getCarreraNombre(nCarrera: number): string {
    const carrera = this.carreras.find(c => c.nCarrera === nCarrera);
    return carrera ? carrera.cNombre : 'Carrera no encontrada';
  }

  resetForm(): void {
    this.user = {
      nombre: '',
      correo: '',
      contrasena: '',
      carrera: 0, // Inicializado como 0
      ciclo: 0, // Inicializado como 0
      clase: '',
      bContraseniaSegura: false
    };
  }

  handleError(message: string): void {
    console.error(message);
    this.errorMessage = message;
  }
}
