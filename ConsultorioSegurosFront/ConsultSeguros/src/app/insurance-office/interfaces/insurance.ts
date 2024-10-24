export interface ResponseJSON {
    message: string;
    data:    Insurance[];
    error:   boolean;
}

export interface Insurance {
    id:            number;
    insuranceName: string;
    insuranceCode: string;
    insuranceAmount: number;
    prima:         number;
}
