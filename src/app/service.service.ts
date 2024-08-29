import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Time } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  private baseUrl = 'https://localhost:44311/api/MacroServicio/getProcedure';

  constructor(private https: HttpClient) { }
  login(usuario: string, contraseña: string): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.Usuario_Login',
      parameters: [
        { parameter: 'cUsuario', value: usuario },
        { parameter: 'cClave', value: contraseña }
      ]
    };
  
    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }

  private getToken(): string | null {
    const name = 'token=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
      let c = ca[i].trim();
      if (c.indexOf(name) === 0) {
        return c.substring(name.length, c.length);
      }
    }
    return null;
  }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    let headers = new HttpHeaders();
      headers = headers.set('Autorizathion', 'Bearer 1|1|1|5|1');
      headers = headers.set('AppSGD', '1|1|1|5|1');

    return headers;
  }

  registro(usuario: string, correo: string, contraseña: string, ContraseniaSegura: boolean): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.InsertarUsuario', 
      parameters: [
        { parameter: 'cUsuario', value: usuario },
        { parameter: 'cCorreo', value: correo },
        { parameter: 'cClave', value: contraseña },
        { parameter: 'bContraseniaSegura', value: ContraseniaSegura },
      ]
    };
  
    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }
  

  listarUsuarios(storedUsuario: number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.Usuario_Listar',
      parameters: [
        { parameter: 'nUsuario', value: storedUsuario}
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  crearproyecto(nombre: string, descripcion: string, fechainicio: string, fechafin: string, UsuarioCreado: number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.CrearProyecto', 
      parameters: [
        { parameter: 'cNombre', value: nombre },
        { parameter: 'cDescripcion', value: descripcion },
        { parameter: 'tFechaInicio', value: fechainicio },
        { parameter: 'tFechaFin', value: fechafin },
        { parameter: 'nUsuarioCreador', value: UsuarioCreado}
      ]
    };
  
    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarProyectos(): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.Proyecto_Listar',
      parameters: []
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  DatoslistarProyectos(proyecto:number, UsuarioCreado:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.Proyecto_ListarDatos',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nUsuarioCreador', value: UsuarioCreado}
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }

  listarCarreras(): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.Proyecto_ListarCarreras',
      parameters: []
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  registroalumnos(usuario: string, correo: string, contraseña: string, carrera: number, ciclo: number, clase:string, ContraseniaSegura: boolean): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.InsertarAlumno', 
      parameters: [
        { parameter: 'cUsuario', value: usuario },
        { parameter: 'cCorreo', value: correo },
        { parameter: 'cClave', value: contraseña },
        { parameter: 'nCarrera', value: carrera },
        { parameter: 'nCiclo', value: ciclo },
        { parameter: 'cClase', value: clase },
        { parameter: 'bContraseniaSegura', value: ContraseniaSegura },
      ]
    };
  
    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }

  crearEntregable(proyecto:number, CantidadTareas:number, Tareas: string, FechaInicio:string, FechaFin:string):Observable<any>{
    const body = {
      procedure: 'Proyectos.dbo.CrearTareas', 
      parameters: [
        { parameter: 'nEntregable', value: proyecto },
        { parameter: 'nCantidadTareas', value: CantidadTareas },
        { parameter: 'cTareas', value: Tareas },
        { parameter: 'tFechaInicio', value: FechaInicio },
        { parameter: 'tFechaFin', value: FechaFin },
      ]
    };
  
    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarEntregables(proyecto:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarEntregablesProyecto',
      parameters: [
        { parameter: 'nProyecto', value: proyecto },
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarUsuariosProyecto(proyecto:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarUsuariosProyecto',
      parameters: [
        { parameter: 'nProyecto', value: proyecto },
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  AsignarUsuarioProyecto(proyecto:number, usuario:number, rol:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.AsignarUsuario',
      parameters: [
        { parameter: 'nProyecto', value: proyecto },
        { parameter: 'nUsuario', value: usuario },
        { parameter: 'nRol', value: rol },
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarProyectosUsuario(usuario:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarProyectoUsuario',
      parameters: [
        { parameter: 'nUsuario', value: usuario},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }

  listardatosEntregable(proyecto:number, entregable:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarDatosEntregable',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nEntregable', value: entregable},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarTareasEntregable(proyecto:number, entregable:number, usuario:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarTareasEntregable',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nEntregable', value: entregable},
        { parameter: 'nUsuario', value: usuario},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  listarArchivosEntregable(proyecto:number, entregable:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarArchivosEntregable',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nEntregable', value: entregable},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }

  listarComentariosEntregable(proyecto:number, entregable:number): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ListarComentariosEntregable',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nEntregable', value: entregable},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }


  ActualizarTarea(proyecto:number, entregable:number, tarea:number, responsable:number, prioridad:string, FechaDeste:string, FechaHasta:string): Observable<any> {
    const body = {
      procedure: 'Proyectos.dbo.ActualizarTarea',
      parameters: [
        { parameter: 'nProyecto', value: proyecto},
        { parameter: 'nEntregable', value: entregable},
        { parameter: 'nTarea', value: tarea},
        { parameter: 'nResponsable', value: responsable},
        { parameter: 'cPrioridad', value: prioridad},
        { parameter: 'tFechaDesde', value: FechaDeste},
        { parameter: 'tFechaHasta', value: FechaHasta},
      ]
    };

    return this.https.post(`${this.baseUrl}`, body, {
      headers: this.getHeaders(),
    });
  }





















    getFile(url: string): Observable<Blob> {
      return this.https.get(url, { responseType: 'blob' });
    }

    cerrarRecorridos(idUsuario: string): Observable<any> {
      const body = {
        procedure: 'Car.dbo.CerrarRecorridosSinFechaFin',
        parameters: [
          { parameter: 'idUsuario', value: idUsuario }
        ]
      };
      return this.https.post(`${this.baseUrl}`, body, {
        headers: this.getHeaders(),
      });
    }

    obtenerRecorridosPorUsuario(idUsuario: string): Observable<any> {
      const body = {
        procedure: 'Car.dbo.ObtenerRecorridosPorUsuario',
        parameters: [
          { parameter: 'idUsuario', value: idUsuario }
        ]
      };
      return this.https.post(`${this.baseUrl}`, body, {
        headers: this.getHeaders(),
      });
    }
}
