import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../service.service'; // Asegúrate de que esta ruta sea correcta

@Component({
  selector: 'app-usuariosprincipal',
  templateUrl: './usuariosprincipal.component.html',
  styleUrls: ['./usuariosprincipal.component.css']
})
export class UsuariosprincipalComponent implements OnInit {

  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario
  datosEntregable: any = {}; // Para almacenar los datos del entregable
  tareas: any[] = []; // Para almacenar las tareas del entregable
  usuarios: any[] = []; // Para almacenar los usuarios
  archivos: any[] = []; // Para almacenar los archivos del entregable
  comentarios: any[] = []; // Para almacenar los comentarios del entregable
  selectedTarea: any = {}; // Para almacenar la tarea seleccionada
  displayDialog: boolean = false; // Para controlar la visibilidad del diálogo

  constructor(private authService: ServiceService) {}

  ngOnInit(): void {
    // Obtener valores de localStorage
    const storedUsuario = localStorage.getItem('cUsuario');
    const storedCorreo = localStorage.getItem('cCorreo');
    const nProyectoStr = localStorage.getItem('nProyecto');
    const nEntregableStr = localStorage.getItem('nEntregable');
    const nUsuarioStr = localStorage.getItem('nUsuario'); // Obtener nUsuario

    // Asignar los valores a las propiedades del componente
    this.cUsuario = storedUsuario ?? 'Usuario no encontrado'; // Usar el operador de fusión nula
    this.cCorreo = storedCorreo ?? 'Correo no encontrado';   // Usar el operador de fusión nula

    // Verificar que nProyecto y nEntregable no sean null y convertirlos a números si es necesario
    const nProyecto = nProyectoStr ? +nProyectoStr : null;
    const nEntregable = nEntregableStr ? +nEntregableStr : null;
    const nUsuario = nUsuarioStr ? +nUsuarioStr : null;

    if (nProyecto && nEntregable) {
      this.authService.listardatosEntregable(nProyecto, nEntregable).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera) {
            this.datosEntregable = response.oContenido.Cabecera[0];
          } else {
            console.error('Error al obtener los datos del entregable:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio:', error.message);
        }
      );

      // Obtener las tareas del entregable
      if (nUsuario) {
        this.authService.listarTareasEntregable(nProyecto, nEntregable, nUsuario).subscribe(
          response => {
            if (response && response.oContenido && response.oContenido.Cabecera) {
              this.tareas = response.oContenido.Cabecera;
            } else {
              console.error('No se encontraron tareas:', response.cMsjError || 'Respuesta inesperada');
            }
          },
          error => {
            console.error('Error en el servicio al obtener tareas:', error.message);
          }
        );
      }

      // Obtener los archivos del entregable
      this.authService.listarArchivosEntregable(nProyecto, nEntregable).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera) {
            this.archivos = response.oContenido.Cabecera;
          } else {
            console.error('No se encontraron archivos:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener archivos:', error.message);
        }
      );

      // Obtener los comentarios del entregable
      this.authService.listarComentariosEntregable(nProyecto, nEntregable).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera) {
            this.comentarios = response.oContenido.Cabecera;
          } else {
            console.error('No se encontraron comentarios:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener comentarios:', error.message);
        }
      );

      // Obtener los usuarios del proyecto
      this.authService.listarUsuariosProyecto(nProyecto).subscribe(
        response => {
          if (response && response.oContenido && response.oContenido.Cabecera) {
            this.usuarios = response.oContenido.Cabecera;
          } else {
            console.error('No se encontraron usuarios:', response.cMsjError || 'Respuesta inesperada');
          }
        },
        error => {
          console.error('Error en el servicio al obtener usuarios:', error.message);
        }
      );
    }
  }

  showDialog(tarea: any): void {
    this.selectedTarea = { ...tarea };
    this.displayDialog = true;
  }

  saveTask(): void {
    const nProyecto = localStorage.getItem('nProyecto');
    const nEntregable = localStorage.getItem('nEntregable');

    const nProyectoNum = nProyecto ? +nProyecto : null;
    const nEntregableNum = nEntregable ? +nEntregable : null;

    if (!nProyectoNum || !nEntregableNum) {
        console.error('nProyecto o nEntregable no encontrados o inválidos');
        return;
    }

    // Validación de los campos de la tarea
    if (
        !this.selectedTarea.nTarea ||
        !this.selectedTarea.cResponsable ||
        !this.selectedTarea.cPrioridad ||
        !this.selectedTarea.FechaDeste ||
        !this.selectedTarea.FechaHasta
    ) {
        console.error('Los datos de la tarea son inválidos o incompletos', this.selectedTarea);
        return;
    }

    // Realizar la solicitud si la validación es exitosa
    this.authService.ActualizarTarea(
        nProyectoNum,
        nEntregableNum,
        +this.selectedTarea.nTarea,
        +this.selectedTarea.cResponsable,
        this.selectedTarea.cPrioridad,
        this.selectedTarea.FechaDeste,
        this.selectedTarea.FechaHasta
    ).subscribe(
        response => {
            console.log('Tarea actualizada:', response);
            this.displayDialog = false;
        },
        error => {
            console.error('Error al actualizar la tarea:', error.message);
        }
    );
}


}
