type DotNet = {
  invokeMethod: (method: string) => void;
}

declare var Blazor: {
  start: (options?: { environment?: string }) => void;
};
