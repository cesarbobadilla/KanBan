import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../service.service'; // Asegúrate de que esta ruta sea correcta

@Component({
  selector: 'app-principal',
  templateUrl: './principal.component.html',
  styleUrls: ['./principal.component.css']
})
export class PrincipalComponent implements OnInit {

  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario

  display: boolean = false;
  project: any = {
    nombre: '',
    descripcion: '',
    fechaInicio: null,
    fechaFin: null
  };

  nUsuarioCreado: number = 0; // Para almacenar el nUsuario

  constructor(private authService: ServiceService) {}

  ngOnInit(): void {
    // Obtener valores de localStorage
    const storedUsuario = localStorage.getItem('cUsuario');
    const storedCorreo = localStorage.getItem('cCorreo');
    const storedUsuarioId = localStorage.getItem('nUsuario'); // Obtener nUsuario

    // Asignar los valores a las propiedades del componente
    this.cUsuario = storedUsuario ? storedUsuario : 'Usuario no encontrado';
    this.cCorreo = storedCorreo ? storedCorreo : 'Correo no encontrado';
    this.nUsuarioCreado = storedUsuarioId ? +storedUsuarioId : 0; // Convertir a número y asignar
  }

  openDialog() {
    this.display = true;
  }

  saveProject() {
    if (this.project.nombre && this.project.descripcion && this.project.fechaInicio && this.project.fechaFin) {
      this.authService.crearproyecto(
        this.project.nombre,
        this.project.descripcion,
        this.project.fechaInicio,
        this.project.fechaFin,
        this.nUsuarioCreado // Pasar el nUsuario
      ).subscribe(
        response => {
          console.log('Proyecto guardado:', response);
          this.display = false; // Cerrar el diálogo
          // Puedes agregar aquí un mensaje de éxito o actualizar la lista de proyectos
        },
        error => {
          console.error('Error al guardar el proyecto:', error);
          // Manejar error aquí
        }
      );
    } else {
      alert('Por favor, complete todos los campos.');
    }
  }
}
