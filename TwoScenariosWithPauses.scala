package computerdatabase

import io.gatling.core.Predef._
import io.gatling.http.Predef._
import scala.concurrent.duration._

class TwoScenariosWithPauses extends Simulation {

  val participantLookup = scenario("Participants").exec(
    http("Participants")
    .post("http://letstest-sporadicload.azurewebsites.net/api/participant?code=tjUVIKi1Bn1ahzKTIOsKu0ARF/xTvK05nF1c2a17RJa9QptuJ675xw==")
    .body(StringBody("""{ "CoCNumber": "22222222" }""")).asJSON
    )

  val companyVerification = scenario("Company").exec(
    http("Company")
    .post("http://letstest-sporadicload.azurewebsites.net/api/company?code=7Hd69sfivolziMPiaJ11F165axGm7VhDpBnLl5hlOO9tMOrMbTeDmQ==")
    .body(StringBody("""{ "CoCNumber": "22222222" }""")).asJSON
    )

  setUp(
    participantLookup.inject(splitUsers(100) into (atOnceUsers(1)) separatedBy (1 minute)),
    companyVerification.inject(splitUsers(100) into (atOnceUsers(1)) separatedBy (1 minute))
    )
}

