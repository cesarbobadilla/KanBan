import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { PrincipalComponent } from './principal/principal.component';
import { LoginComponent } from './login/login.component';
import { CalendarModule } from 'primeng/calendar';
import { HttpClientModule } from '@angular/common/http';

// Importaciones de PrimeNG
import { CardModule } from 'primeng/card';
import { InputText, InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { AdministradorComponent } from './administrador/administrador.component';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { GestorComponent } from './gestor/gestor.component';
import { GestorproyectosComponent } from './gestorproyectos/gestorproyectos.component';
import { DatosgestorproyectosComponent } from './datosgestorproyectos/datosgestorproyectos.component';
import { UsuariosprincipalComponent } from './usuariosprincipal/usuariosprincipal.component';
import { UsuariosasiganarliderComponent } from './usuariosasiganarlider/usuariosasiganarlider.component';
import { UsuariosasignarComponent } from './usuariosasignar/usuariosasignar.component';


@NgModule({
  declarations: [
    AppComponent,
    PrincipalComponent,
    LoginComponent,
    AdministradorComponent,
    GestorComponent,
    GestorproyectosComponent,
    DatosgestorproyectosComponent,
    UsuariosprincipalComponent,
    UsuariosasiganarliderComponent,
    UsuariosasignarComponent
  ],
  imports: [
    BrowserModule,
    CardModule,
    InputTextModule,
    CalendarModule,
    DialogModule,
    ButtonModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
    BrowserAnimationsModule,
    TableModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
