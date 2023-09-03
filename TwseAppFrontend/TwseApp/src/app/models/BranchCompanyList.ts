interface BranchCompany {
  id: number;
  dateOfPublication: string;
  securitiesFirmCode: string;
  securitiesFirmName: string;
  openingDate: string;
  address: string;
  telephone: string;
  code: string;
}
  
export interface BranchCompanyList {
  branchCompanies: BranchCompany[];
  totalNumber: number;
}