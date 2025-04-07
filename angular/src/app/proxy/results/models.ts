
export interface ListResult<T> extends Result<T> {
  values: T[];
}

export interface Result<T> {
  succeeded: boolean;
  errorCode?: string;
  errorMessage?: string;
  isFailed: boolean;
  warning: boolean;
  isWarned: boolean;
  value: T;
}
