import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../service.service';

@Component({
  selector: 'app-datosgestorproyectos',
  templateUrl: './datosgestorproyectos.component.html',
  styleUrls: ['./datosgestorproyectos.component.css']
})
export class DatosgestorproyectosComponent implements OnInit {

  proyecto: any = {};  // Para almacenar los datos del proyecto
  entregables: any[] = [];  // Para almacenar los entregables del proyecto
  usuarios: any[] = [];  // Para almacenar los usuarios del proyecto
  datosUsuarios: any[] = [];
  selectedUsuario: any;  // Usuario seleccionado
  selectedRol: string = '';  // Rol seleccionado
  displayDialog: boolean = false; // Para controlar la visibilidad del diálogo

  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario

  constructor(private router: Router, private authService: ServiceService) {}

  ngOnInit(): void {
    // Obtener valores de localStorage
    const storedUsuario = localStorage.getItem('cUsuario');
    const storedCorreo = localStorage.getItem('cCorreo');

    // Asignar los valores a las propiedades del componente
    this.cUsuario = storedUsuario ? storedUsuario : 'Usuario no encontrado';
    this.cCorreo = storedCorreo ? storedCorreo : 'Correo no encontrado';

    // Obtener nProyecto y nUsuarioCreador de localStorage
    const nProyecto = localStorage.getItem('nProyecto');
    const nUsuarioCreador = localStorage.getItem('nUsuarioCreador');

    if (nProyecto && nUsuarioCreador) {
      // Llamar al servicio para obtener los datos del proyecto
      this.authService.DatoslistarProyectos(+nProyecto, +nUsuarioCreador).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera && response.oContenido.Cabecera.length > 0) {
            this.proyecto = response.oContenido.Cabecera[0];
          } else {
            console.error('Error al obtener los datos del proyecto:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio:', error.message);
        }
      );

      // Llamar al servicio para obtener los entregables del proyecto
      this.authService.listarEntregables(+nProyecto).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera && response.oContenido.Cabecera.length > 0) {
            this.entregables = response.oContenido.Cabecera;
          } else {
            console.error('No se encontraron entregables:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener entregables:', error.message);
        }
      );

      // Llamar al servicio para obtener los usuarios del proyecto
      this.authService.listarUsuariosProyecto(+nProyecto).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera && response.oContenido.Cabecera.length > 0) {
            this.usuarios = response.oContenido.Cabecera;
          } else {
            console.error('No se encontraron usuarios:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener usuarios:', error.message);
        }
      );
    } else {
      console.error('nProyecto o nUsuarioCreador no encontrados en localStorage');
    }
  }

  showDialog() {
    // Obtener usuarios cuando se abre el diálogo
    const storedUsuario = localStorage.getItem('nUsuarioCreador');
    if (storedUsuario) {
      this.authService.listarUsuarios(+storedUsuario).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera) {
            this.datosUsuarios = response.oContenido.Cabecera;
            this.displayDialog = true;
          } else {
            console.error('No se encontraron usuarios:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener usuarios:', error.message);
        }
      );
    } else {
      console.error('nUsuarioCreador no encontrado en localStorage');
    }
  }

  hideDialog() {
    this.displayDialog = false;
  }

  save() {
    if (this.selectedUsuario && this.selectedRol) {
      // Obtener nProyecto desde localStorage
      const nProyecto = localStorage.getItem('nProyecto');

      if (nProyecto) {
        // Convertir selectedRol a un valor numérico si es necesario (puede ser un enum o un ID)
        const rol = this.selectedRol === 'Lider' ? 3 : 4; // Ajusta según cómo necesites mapear los roles

        // Llamar al servicio para asignar el usuario al proyecto
        this.authService.AsignarUsuarioProyecto(+nProyecto, +this.selectedUsuario, rol).subscribe(
          response => {
            if (response && response.oContenido && response.oContenido.Cabecera) {
              console.log('Usuario asignado con éxito');
              this.hideDialog(); // Ocultar el diálogo después de guardar
            } else {
              console.error('Error al asignar usuario:', response.cMsjError || 'Respuesta inesperada');
            }
          },
          error => {
            console.error('Error en el servicio al asignar usuario:', error.message);
          }
        );
      } else {
        console.error('nProyecto no encontrado en localStorage');
      }
    } else {
      console.error('Debe seleccionar un usuario y un rol');
    }
  }
}
