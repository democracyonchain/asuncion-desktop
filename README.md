<div align="center">
  <a href="https://blockfrost.io/" target="_blank">    
    <img src="https://avatars.githubusercontent.com/u/9141961?s=200&v=4" width="190" alt="BlockFrost Logo" />
  </a>
</div>
<div align="center"> 
  <a href="https://dotnet.microsoft.com/apps/aspnet" target="_blank">
    <img src="https://img.shields.io/badge/.NET Platform-8.0-blue" alt=".NET Framework Version">
  </a>  
</div>

### English Version

# Desktop Module for Blockchain Voting System  
**Actors**
* David Tacuri

### Summary  

The desktop module for the Blockchain Voting System performs the following tasks:
1. Scans scrutiny records (actas de escrutinio).
2. Recognizes the QR code from the scrutiny records.
3. Splits the record into segments (each segment corresponds to the votes of each candidate).
4. Uses AI (machine learning) for intelligent recognition of the votes received by each candidate based on Microsoft's Machine Learning technology.
5. Sends the images to IPFS using the `asuncion-backend` API.

### Key Features

- **Desktop Application**: Scans scrutiny records.
- **QR Code Recognition**: Detects QR codes on the records.
- **Segmentation**: Splits the record into segments corresponding to the votes of each candidate.
- **AI Vote Recognition**: Uses Microsoft's Machine Learning for intelligent vote recognition.
- **IPFS Integration**: Sends images to IPFS using the backend API.

### Technical Aspects

- **Programming Language**: C#
- **Framework**: .NET Framework 4.7.2

### Tools and Libraries
1. **Twain Controller**: For scanning functionality.
2. **ZXing Component**: For QR code reading.
3. **Machine Learning Model**: `mlAsuncionModel` for intelligent vote recognition.
4. **API Integration**: For sending data to IPFS and interacting with the blockchain.

### Configuration

Ensure that the following configuration file is adapted for your environment:

```xml
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
    <appSettings>
        <add key="ApiBaseUrl" value="http://localhost:3090/" />
        <add key="ApiBaseUrlIpfs" value="http://localhost:5116/api/Ipfs/" />
        <add key="ApiBaseUrlCardano" value="http://localhost:5116/api/Cardano/" />
        <add key="AnotherSetting" value="SomeValue" />
        <add key="MaxPage" value="20" />
        <add key="BasePath" value="C:/AsuncionImage" />
        <add key="ReferenciaX" value="300" />
        <add key="ReferenciaY" value="600" />
        <add key="TimeOut" value="10" />
    </appSettings>
</configuration>
```

### Components Used

1. **Twain Controller**: For document scanning.
2. **ZXing Component**: For reading QR codes.
3. **Machine Learning Model**: `mlAsuncionModel` for intelligent vote recognition.

### How to Create and Configure `mlAsuncionModel`

To create and configure the machine learning model `mlAsuncionModel` in Visual Studio 2022, follow these steps:

1. **Open the project in Visual Studio 2022**.
2. **Right-click on the project in Solution Explorer** and select **Add > Machine Learning**.
3. **Name the model** `mlAsuncionModel`.
4. In the **Model Wizard**, select **Image Classification** as the type of model to create.
5. **Upload training data**: You will need to provide a dataset of labeled images for training the model (e.g., images of votes for different candidates).
6. **Train the model** using the dataset. Visual Studio will guide you through the process of configuring the training settings.
7. **Evaluate the model**: Once the training is complete, evaluate the model's accuracy.
8. **Save and integrate**: Save the trained model and integrate it into your application for use in recognizing votes from scanned images.

### Prerequisites

- Visual Studio .NET 2022
- .NET Framework 4.7.2

### Installation Procedure

1. Clone the repository:  
```plaintext
$ git clone https://github.com/democracyonchain/asuncion-desktop.git
```
2. Open the solution in Visual Studio 2022.
3. Set up the required machine learning model (`mlAsuncionModel`) using the image classification template in Visual Studio.
4. Build the solution in Visual Studio.
5. Run the application.

### Download Link

Download the project from the following repository:  
[https://github.com/democracyonchain/asuncion-desktop.git](https://github.com/democracyonchain/asuncion-desktop.git)

******************************************************************************************
### Spanish Version


# Módulo de Escritorio para el Sistema de Votación en Blockchain  
**Desarrollador**: David Tacuri

### Resumen  

El módulo de escritorio para el Sistema de Votación en Blockchain realiza las siguientes tareas:
1. Escanea actas de escrutinio.
2. Reconoce el código QR de las actas de escrutinio.
3. Corta las actas en segmentos (cada segmento corresponde a los votos de cada candidato).
4. Utiliza IA (machine learning) para el reconocimiento inteligente de los votos recibidos por cada candidato, basado en la tecnología de Machine Learning de Microsoft.
5. Envía las imágenes a IPFS usando la API `asuncion-backend`.

### Características Principales

- **Aplicación de Escritorio**: Escanea las actas de escrutinio.
- **Reconocimiento de Código QR**: Detecta los códigos QR en las actas.
- **Segmentación**: Divide las actas en segmentos correspondientes a los votos de cada candidato.
- **Reconocimiento Inteligente de Votos**: Usa Machine Learning de Microsoft para el reconocimiento de votos.
- **Integración con IPFS**: Envía las imágenes a IPFS utilizando la API de backend.

### Aspectos Técnicos

- **Lenguaje de Programación**: C#
- **Framework**: .NET Framework 4.7.2

### Herramientas y Bibliotecas Utilizadas
1. **Controlador Twain**: Para la funcionalidad de escaneo.
2. **Componente ZXing**: Para la lectura de códigos QR.
3. **Modelo de Machine Learning**: `mlAsuncionModel` para el reconocimiento inteligente de votos.
4. **Integración con API**: Para enviar datos a IPFS e interactuar con la blockchain.

### Configuración

Asegúrese de adaptar el siguiente archivo de configuración para su entorno:

```xml
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
    </startup>
    <appSettings>
        <add key="ApiBaseUrl" value="http://localhost:3090/" />
        <add key="ApiBaseUrlIpfs" value="http://localhost:5116/api/Ipfs/" />
        <add key="ApiBaseUrlCardano" value="http://localhost:5116/api/Cardano/" />
        <add key="AnotherSetting" value="SomeValue" />
        <add key="MaxPage" value="20" />
        <add key="BasePath" value="C:/AsuncionImage" />
        <add key="ReferenciaX" value="300" />
        <add key="ReferenciaY" value="600" />
        <add key="TimeOut" value="10" />
    </appSettings>
</configuration>
```

### Componentes Utilizados

1. **Controlador Twain**: Para el escaneo de documentos.
2. **Componente ZXing**: Para la lectura de códigos QR.
3. **Modelo de Machine Learning**: `mlAsuncionModel` para el reconocimiento inteligente de votos.

### Cómo Crear y Configurar `mlAsuncionModel`

Para crear y configurar el modelo de machine learning `mlAsuncionModel` en Visual Studio 2022, siga estos pasos:

1. **Abra el proyecto en Visual Studio 2022**.
2. **Haga clic derecho en el proyecto en el Explorador de Soluciones** y seleccione **Agregar > Machine Learning**.
3. **Nombre el modelo** `mlAsuncionModel`.
4. En el **Asistente de Modelos**, seleccione **Clasificación de Imágenes** como el tipo de modelo a crear.
5. **Cargue los datos de entrenamiento**: Necesitará proporcionar un conjunto de imágenes etiquetadas para entrenar el modelo (por ejemplo, imágenes de votos para diferentes candidatos).
6. **Entrene el modelo** usando el conjunto de datos. Visual Studio le guiará a través del proceso de configuración de los ajustes de entrenamiento.
7. **Evalue el modelo**: Una vez completado el entrenamiento, evalúe la precisión del modelo.
8. **Guarde e integre**: Guarde el modelo entrenado e intégralo en su aplicación para usarlo en el reconocimiento de votos de las imágenes escaneadas.

### Requisitos Previos

- Visual Studio .NET 2022
- .NET Framework 4.7.2

### Procedimiento de Instalación

1. Clonar el repositorio:  
```plaintext
$ git clone https://github.com/democracyonchain/asuncion-desktop.git
```
2. Abra la solución en Visual Studio 2022.
3. Configure el modelo de machine learning requerido (`mlAsuncionModel`) utilizando la plantilla de clasificación de imágenes en Visual Studio.
4. Compile la solución en Visual Studio.
5. Ejecute la aplicación.

### Enlace de Descarga

Descargue el proyecto desde el siguiente repositorio:  
[https://github.com/democracyonchain/asuncion-desktop.git](https://github.com/democracyonchain/asuncion-desktop.git)



