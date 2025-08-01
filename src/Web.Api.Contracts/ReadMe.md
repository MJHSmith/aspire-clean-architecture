A lightweight .Net Standard 2.1 library with API contract definitions.  
Other .NET projects can reference this library to access the API contract definitions.


Specifies if a parameter is required (nullable is for IDE only).  
Does not specify any other requirements (MaxLength, uniqueness etc.), these are dynamically 
determined by the validation in the CQRS pipeline.  
If validation rules needs to be communicated to consumer, will have to be done via 
- OpenAPI documentation on endpoint
- Help on CLI tool
- Form validation on front ends