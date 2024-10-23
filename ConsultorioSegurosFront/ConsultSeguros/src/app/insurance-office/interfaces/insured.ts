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
    insured:        Insured
    insurances:     Insurance[];
}