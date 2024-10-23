import { Insurance } from "./insurance";

export interface ResponseJSON {
    message: string;
    data:    Insured[];
    error:   boolean;
}

export interface Insured {
    id:             number;
    identification: string;
    insuredName:    string;
    phoneNumber:    string;
    age:            number;
}

export interface InsuredDTO {
    id:             number;
    identification: string;
    insuredName:    string;
    phoneNumber:    string;
    age:            number;
    insurances:     string[];
}