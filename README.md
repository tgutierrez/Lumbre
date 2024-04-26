Simple, embeddable, low ambition but overcomplicated FHIR middleware.

When (and if) it's completed, it will be an extensible middleware between FHIR requests and the host's application data.

It's also an attempt of a "clean-ish" architecture, with the intention of decoupling as much aspects as possible. 

It based on .NET 8 and Mediatr.

Currently supported persistance methods:
- MongoDB
- In Memory Test Persistance.

Currently supported operations:
- Query by ID
- Insert single

Currently supported presentation methods:
- FHIR Object
- Raw Json

...but above all, this is my attempt to learn the intrincacies of an extensible architecture and the inner workings of Mediatr.
