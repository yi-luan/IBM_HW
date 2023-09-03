interface Headquarters {
    id: string;
    name: string;
    establishmentDate: string;
    address: string;
    telephone: string;
}

interface Branch {
    id: number;
    dateOfPublication: string;
    securitiesFirmCode: string;
    securitiesFirmName: string;
    openingDate: string;
    address: string;
    telephone: string;
    code: string;
    headquarters: Headquarters;
}

interface Headquarters {
    id: string;
    name: string;
    establishmentDate: string;
    address: string;
    telephone: string;
}

export interface AllCompanyData {
    status: boolean;
    message: string | null;
    data: {
        branchList: Branch[];
        headquarterList:Headquarters[];
        branchTotalCount:number;
        headerquartersTotalCount:number;
    };
}

export interface CheckIfDatabaseHaveBeenInitResponse {
    status: boolean;
    message: string | null;
    data: object | null;
}







