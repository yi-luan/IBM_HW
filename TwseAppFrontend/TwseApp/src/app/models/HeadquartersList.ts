export interface Headquarters {
    id: string;
    name: string;
    establishmentDate: string;
    address: string;
    telephone: string;
  }
  
  export interface HeadquartersList {
    headquarters: Headquarters[];
    totalNumber: number;
  }