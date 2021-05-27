export function DateToString(data: any){
    data = new Date(data);
    var day = data.getDate().toString().padStart(2, "0");
    var month = (1+ data.getMonth()).toString().padStart(2, "0");
    var year = 1900 + data.getYear();
    var answer = day+"/"+month+"/"+year;
    return answer;
}
export function DateToStringGuion(data: any){
    var day = data.getDate().toString().padStart(2, "0");
    var month = (1+ data.getMonth()).toString().padStart(2, "0");
    var year = 1900 + data.getYear();
    var answer = year+"-"+month+"-"+day;
    return answer;
}

export function StringToDate(data: any){
    return new Date(data);
}