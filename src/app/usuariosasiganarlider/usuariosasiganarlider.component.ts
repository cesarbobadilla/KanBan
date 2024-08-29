import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../service.service';

@Component({
  selector: 'app-usuariosasiganarlider',
  templateUrl: './usuariosasiganarlider.component.html',
  styleUrls: ['./usuariosasiganarlider.component.css']
})
export class UsuariosasiganarliderComponent implements OnInit {

  
  cUsuario: string = '';  // Para mostrar el nombre del usuario
  cCorreo: string = '';   // Para mostrar el correo del usuario
  proyectos: any[] = [];  // Para almacenar los proyectos obtenidos

  constructor(private router: Router, private authService: ServiceService) {}

  ngOnInit(): void {
    // Obtener valores de localStorage
    const storedUsuario = localStorage.getItem('cUsuario');
    const storedCorreo = localStorage.getItem('cCorreo');

    // Asignar los valores a las propiedades del componente
    this.cUsuario = storedUsuario ? storedUsuario : 'Usuario no encontrado';
    this.cCorreo = storedCorreo ? storedCorreo : 'Correo no encontrado';

    // Obtener los proyectos
    this.loadProjects();
  }
  
  mostrarDialogo: boolean = false;
  tarea: any = {
    entregable: '',
    cantidad: 0,
    fechaInicio: '',
    fechaFin: ''
  };
  tareas: any[] = [];

  mostrarDialogoAsignarTareas() {
    this.mostrarDialogo = true;
  }

  generarInputsTareas() {
    this.tareas = [];
    for (let i = 0; i < this.tarea.cantidad; i++) {
      this.tareas.push({ descripcion: '' });
    }
  }
  
  goToDatosProyecto(nProyecto: string, nUsuarioCreador: string) {
    // Guardar en localStorage
    localStorage.setItem('nProyecto', nProyecto);
    localStorage.setItem('nUsuarioCreador', nUsuarioCreador);

    // Redirigir a la página de datos del proyecto
    this.router.navigate(['/datosliderproyecto']);
  }

  guardarTareas() {
    const nProyecto = parseInt(localStorage.getItem('nProyecto')!, 10);  // Asegurarse de que nProyecto es un número
    const CantidadTareas = this.tarea.cantidad;
    const FechaInicio = this.tarea.fechaInicio;
    const FechaFin = this.tarea.fechaFin;
    
    // Concatenar las descripciones de las tareas en una sola cadena
    const Tareas = this.tareas.map(t => t.descripcion).join(';');
  
    // Llamar al método del servicio para crear el entregable
    this.authService.crearEntregable(nProyecto, CantidadTareas, Tareas, FechaInicio, FechaFin).subscribe(
      response => {
        console.log('Tareas guardadas:', response);
        this.mostrarDialogo = false;
        // Aquí podrías agregar alguna lógica para manejar la respuesta, como mostrar una notificación o actualizar la lista de tareas.
      },
      error => {
        console.error('Error al guardar las tareas:', error);
      }
    );
  }
  

  loadProjects(): void {
    const storedUsuario = localStorage.getItem('nUsuario');
    this.authService.listarProyectosUsuario(Number(storedUsuario)).subscribe(
      response => {
        console.log('Respuesta en loadProjects:', response);
        if (response && response.oContenido && response.oContenido.Cabecera) {
          this.proyectos = response.oContenido.Cabecera;
        } else {
          console.error('Error al listar los proyectos:', response.cMsjError || 'Respuesta inesperada');
        }
      },
      error => {
        console.error('Error en el servicio:', error.message);
      }
    );
  }

}
