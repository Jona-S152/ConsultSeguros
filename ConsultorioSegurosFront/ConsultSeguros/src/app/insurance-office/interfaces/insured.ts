import { Insurance } from "./insurance";

export interface ResponseJSON {
    message: string;
    data:    InsuredGet[];
    error:   boolean;
}

export interface InsuredGet {
    id:             number;
    identification: string;
    insuredName:    string;
    phoneNumber:    string;
    age:            number;
}

export interface Insured {
    id:             number;
    identification: string;
    insuredName:    string;
    phoneNumber:    string;
    age:            number;
    insurancesIds:  string;
}

export interface InsuredDTO {
    id:             number;
    identification: string;
    insuredName:    string;
    phoneNumber:    string;
    age:            number;
    insurances:     string[];
}