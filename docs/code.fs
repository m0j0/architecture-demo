type User = { Id : int; Name : string }

type CreateUserResult =
  | User
  | EmailAlreadyRegistered
  | ParentNotFound
  | ParentNotFound2
  
let matcher x =
  match x with
  | User -> printfn "User case"
  | EmailAlreadyRegistered -> printfn "EmailAlreadyRegistered case"
  | ParentNotFound -> printfn "point"

TODO
match x with
| User -> printfn "User case"
| EmailAlreadyRegistered -> printfn "EmailAlreadyRegistered case"
| ParentNotFound -> printfn "point"

SqlException


class CreateUserResult
EmailAlreadyRegisteredException
ParentNotFoundException
